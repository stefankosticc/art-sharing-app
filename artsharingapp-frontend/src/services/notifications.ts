import * as signalR from "@microsoft/signalr";
import { BACKEND_BASE_URL } from "../config/constants";
import authAxios from "./authAxios";
import { NotificationStatus } from "./enums";

export interface NotificationResponse {
  id: number;
  text: string;
  createdAt: string;
  status: NotificationStatus;
}

export async function getNotifications(
  skip = 0,
  take = 10,
): Promise<NotificationResponse[]> {
  const response = await authAxios.get(`notifications`, {
    params: { skip, take },
  });
  return response.data;
}

export async function getUnreadNotificationCount(): Promise<number> {
  const response = await authAxios.get(`notifications/unread-count`);
  return response.data.count;
}

export async function markNotificationAsRead(
  notificationId: number,
): Promise<boolean> {
  try {
    await authAxios.put(`notification/${notificationId}/read`);
    return true;
  } catch (error: any) {
    const message =
      error?.response?.data?.error ||
      error?.message ||
      "An unknown error occurred.";
    console.error("Error:", message);
    return false;
  }
}

export async function deleteNotification(
  notificationId: number,
): Promise<boolean> {
  try {
    await authAxios.put(`notification/${notificationId}/delete`);
    return true;
  } catch (error: any) {
    const message =
      error?.response?.data?.error ||
      error?.message ||
      "An unknown error occurred.";
    console.error("Error:", message);
    return false;
  }
}

const NOTIFICATION_HUB_URL = `${BACKEND_BASE_URL}/hubs/notifications`;

type NotificationListener = (notification: NotificationResponse | null) => void;

class NotificationService {
  private connection: signalR.HubConnection | null = null;
  private started = false;
  private unreadCount = 0;
  private listeners = new Set<NotificationListener>();

  getUnreadCount = () => this.unreadCount;

  subscribe = (listener: NotificationListener) => {
    this.listeners.add(listener);
    return () => {
      this.listeners.delete(listener);
    };
  };

  private emit(notification: NotificationResponse | null) {
    this.listeners.forEach((listener) => listener(notification));
  }

  decrementUnread = () => {
    if (this.unreadCount === 0) return;
    this.unreadCount -= 1;
    this.emit(null);
  };

  async start(accessToken: string) {
    if (this.started) return;
    this.started = true;

    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(NOTIFICATION_HUB_URL, {
        accessTokenFactory: () => accessToken,
      })
      .withAutomaticReconnect()
      .build();

    this.connection.on(
      "ReceiveNotification",
      (notification: NotificationResponse) => {
        this.unreadCount += 1;
        this.emit(notification);
      },
    );

    try {
      await this.connection.start();
      this.unreadCount = await getUnreadNotificationCount();
      this.emit(null);
    } catch (error) {
      console.error("Notification Connection Error:", error);
      this.started = false;
    }
  }

  stop() {
    this.connection?.stop();
    this.connection = null;
    this.started = false;
    this.unreadCount = 0;
    this.emit(null);
  }
}

export default new NotificationService();
