import * as signalR from "@microsoft/signalr";
import { BACKEND_BASE_URL } from "../config/constants";
import authAxios from "./authAxios";

const CHAT_HUB_URL = `${BACKEND_BASE_URL}/hubs/chat`;

export interface ChatMessage {
  id: number;
  senderId: number;
  receiverId: number;
  message: string;
  sentAt: string;
  isRead: boolean;
}

export interface ChatUser {
  userId: number;
  name: string;
  userName: string;
  profilePhoto: string | null;
  unreadMessageCount: number;
  lastMessage: string;
  lastMessageDateTime: string;
}

class ChatService {
  private connection: signalR.HubConnection | null = null;

  connect(accessToken: string) {
    if (this.connection) return this.connection;

    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(CHAT_HUB_URL, {
        accessTokenFactory: () => accessToken,
      })
      .withAutomaticReconnect()
      .build();

    return this.connection;
  }

  async start(accessToken: string) {
    try {
      if (!this.connection) {
        this.connect(accessToken);
      }
      if (this.connection?.state !== signalR.HubConnectionState.Connected) {
        await this.connection?.start();
      }
    } catch (error) {
      console.error("Chat Connection Error:", error);
    }
  }

  onReceiveMessage(callback: (message: any) => void) {
    this.connection?.on("ReceiveMessage", callback);
  }

  onMessageSent(callback: (message: any) => void) {
    this.connection?.on("MessageSent", callback);
  }

  async sendMessage(receiverId: number, message: string) {
    await this.connection?.invoke("SendMessage", { receiverId, message });
  }

  async getChatHistory(
    otherUserId: number,
    skip: number = 0,
    take: number = 50
  ) {
    return await this.connection?.invoke(
      "GetChatHistory",
      otherUserId,
      skip,
      take
    );
  }

  async markAsRead(messageId: number) {
    await this.connection?.invoke("MarkAsRead", messageId);
  }

  async getConversations(skip: number = 0, take: number = 20) {
    const response = await authAxios.get("/chat/conversations", {
      params: { skip, take },
    });
    return response.data;
  }

  stop() {
    this.connection?.stop();
    this.connection = null;
  }
}

export default new ChatService();
