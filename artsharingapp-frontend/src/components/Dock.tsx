import { useTranslation } from "react-i18next";
import { FiSearch } from "react-icons/fi";
import { FaMap, FaUser, FaPlusSquare } from "react-icons/fa";
import { NavLink } from "react-router-dom";
import "../styles/Dock.css";
import { FaWindowMaximize } from "react-icons/fa6";
import { MdOutlineChatBubble } from "react-icons/md";
import { IoNotifications } from "react-icons/io5";
import { useEffect, useState } from "react";
import Search from "./search/Search";
import Notifications from "./Notifications";
import { useLoggedInUser } from "../hooks/useLoggedInUser";
import { useUnreadNotifications } from "../hooks/useUnreadNotifications";

const Dock = () => {
  const { t } = useTranslation();
  const [isSearchOpen, setIsSearchOpen] = useState<boolean>(false);
  const [isNotificationsOpen, setIsNotificationsOpen] =
    useState<boolean>(false);

  const { loggedInUser } = useLoggedInUser();
  const { unreadCount, decrementUnread } = useUnreadNotifications();

  useEffect(() => {
    const handleKeyDown = (e: KeyboardEvent) => {
      if (e.key === "Escape") {
        setIsSearchOpen(false);
      }
    };

    if (isSearchOpen) {
      window.addEventListener("keydown", handleKeyDown);
    }

    return () => {
      window.removeEventListener("keydown", handleKeyDown);
    };
  }, [isSearchOpen]);

  return (
    <>
      {isSearchOpen && <Search onClose={() => setIsSearchOpen(false)} />}
      <div className="dock">
        <div className="notifications-container">
          <IoNotifications
            id="notifications-icon"
            onClick={(e) => {
              e.stopPropagation();
              if (isSearchOpen) setIsSearchOpen(false);
              setIsNotificationsOpen(!isNotificationsOpen);
            }}
          />
          {unreadCount > 0 && (
            <span className="notifications-badge">
              {unreadCount > 99 ? "99+" : unreadCount}
            </span>
          )}
          {isNotificationsOpen && (
            <Notifications
              onClose={() => setIsNotificationsOpen(false)}
              onNotificationRead={decrementUnread}
            />
          )}
        </div>
        <NavLink to={"/map"} title={t("common.dock.map")}>
          <FaMap />
        </NavLink>
        <NavLink to={"/discover"} title={t("common.dock.discover")}>
          <FaWindowMaximize />
        </NavLink>
        <div
          onClick={(e) => {
            e.stopPropagation();
            if (isNotificationsOpen) setIsNotificationsOpen(false);
            setIsSearchOpen(!isSearchOpen);
          }}
        >
          <FiSearch id="search-icon" />
        </div>
        <NavLink to={`/${loggedInUser?.userName}`} title={t("common.dock.profile")}>
          <FaUser />
        </NavLink>
        <NavLink to={"/artwork/new"} title={t("common.dock.newArtwork")}>
          <FaPlusSquare />
        </NavLink>
        <NavLink to={"/chat"} title={t("common.dock.chat")}>
          <MdOutlineChatBubble />
        </NavLink>
      </div>
    </>
  );
};

export default Dock;
