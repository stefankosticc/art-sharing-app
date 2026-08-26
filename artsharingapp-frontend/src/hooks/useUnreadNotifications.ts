import { useState, useEffect } from "react";
import { toast } from "react-toastify";
import notificationService from "../services/notifications";

export const useUnreadNotifications = () => {
  const [unreadCount, setUnreadCount] = useState(
    notificationService.getUnreadCount(),
  );

  useEffect(() => {
    const accessToken = localStorage.getItem("accessToken");
    if (accessToken) notificationService.start(accessToken);

    return notificationService.subscribe((notification) => {
      setUnreadCount(notificationService.getUnreadCount());
      if (notification) toast.info(notification.text);
    });
  }, []);

  return {
    unreadCount,
    decrementUnread: notificationService.decrementUnread,
  };
};
