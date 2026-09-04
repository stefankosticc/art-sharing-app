import { useRef } from "react";
import { useTranslation } from "react-i18next";
import { useNotifications } from "../hooks/useNotifications";
import { NotificationStatus } from "../services/enums";
import "../styles/Notifications.css";
import { BsCheck2All } from "react-icons/bs";
import { FaTrashAlt } from "react-icons/fa";
import {
  deleteNotification,
  markNotificationAsRead,
  NotificationResponse,
} from "../services/notifications";
import { useClickOutside } from "../hooks/useClickOutside";
import { useScroll } from "../hooks/useScroll";

interface NotificationsProps {
  onClose: () => void;
  onNotificationRead: () => void;
}

const Notifications = ({ onClose, onNotificationRead }: NotificationsProps) => {
  const { t } = useTranslation();
  const {
    notifications,
    loadingNotifications,
    loadMoreNotifications,
    markAsReadLocally,
    deleteLocally,
  } = useNotifications();

  const notificationMenuRef = useRef<HTMLDivElement>(null);
  useClickOutside(notificationMenuRef, onClose);

  useScroll({
    ref: notificationMenuRef,
    storageKey: "notificationsScrollY",
    onReachBottom: loadMoreNotifications,
  });

  const persistScrollPosition = () => {
    const container = notificationMenuRef.current;
    if (container) {
      sessionStorage.setItem(
        "notificationsScrollY",
        container.scrollTop.toString()
      );
    }
  };

  const handleRead = async (notification: NotificationResponse) => {
    persistScrollPosition();
    const success = await markNotificationAsRead(notification.id);
    if (success) {
      markAsReadLocally(notification.id);
      if (notification.status === NotificationStatus.UNREAD)
        onNotificationRead();
    }
  };

  const handleDelete = async (notification: NotificationResponse) => {
    persistScrollPosition();
    const success = await deleteNotification(notification.id);
    if (success) {
      deleteLocally(notification.id);
      if (notification.status === NotificationStatus.UNREAD)
        onNotificationRead();
    }
  };

  return (
    <div className="notifications-menu" ref={notificationMenuRef}>
      {notifications.length === 0 && !loadingNotifications ? (
        <p className="notifications-no-results">
          {t("notifications.noNotificationsFound")}
        </p>
      ) : (
        <>
          {notifications.map((notification) => (
            <div
              key={notification.id}
              className={`notification ${
                notification.status === NotificationStatus.UNREAD
                  ? "notification-unread"
                  : ""
              }`}
            >
              <p>{notification.text}</p>
              <div className="notification-actions">
                <div
                  className="notification-icon-wrapper"
                  onClick={() => handleRead(notification)}
                >
                  <BsCheck2All title={t("notifications.markAsRead")} />
                </div>
                <div
                  className="notification-icon-wrapper"
                  onClick={() => handleDelete(notification)}
                >
                  <FaTrashAlt title={t("common.delete")} />
                </div>
              </div>

              {notification.status === NotificationStatus.UNREAD && (
                <div className="notification-dot"></div>
              )}
            </div>
          ))}

          {loadingNotifications && (
            <div className="notifications-no-results notifications-loader" />
          )}
        </>
      )}
    </div>
  );
};

export default Notifications;
