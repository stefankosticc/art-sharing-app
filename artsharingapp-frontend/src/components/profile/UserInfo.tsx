import { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import { MdOutlineChatBubble } from "react-icons/md";
import { formatFollowCount } from "../../utils/formatting";
import {
  ARTIST_FALLBACK_IMAGE,
  IMAGE_SERVICE_BASE_URL,
} from "../../config/constants";
import "./styles/UserInfo.css";
import { User } from "../../services/auth";
import { followUser, unfollowUser } from "../../services/followers";
import { IoSettingsOutline } from "react-icons/io5";
import SettingsModal from "./SettingsModal";
import FollowersModal from "./FollowersModal";
import { useNavigate } from "react-router-dom";

interface UserInfoProps {
  user: User | null;
  setUser: React.Dispatch<React.SetStateAction<User | null>>;
  triggerRefetchUser: () => void;
  loading: boolean;
  loggedInUser: User | null;
  isMyProfile: boolean;
}

const UserInfo = ({
  user,
  setUser,
  triggerRefetchUser,
  loading,
  loggedInUser,
  isMyProfile,
}: UserInfoProps) => {
  const { t } = useTranslation();
  const [imgSrc, setImgSrc] = useState<string>();
  const [isFollowing, setIsFollowing] = useState<boolean>(false);
  const [isSettingsOpen, setIsSettingsOpen] = useState<boolean>(false);
  const [isFollowersModalOpen, setIsFollowersModalOpen] = useState<{
    isOpen: boolean;
    tab?: "followers" | "following";
  }>({ isOpen: false, tab: "followers" });

  const navigate = useNavigate();

  useEffect(() => {
    if (user?.profilePhoto) {
      setImgSrc(`${IMAGE_SERVICE_BASE_URL}${user.profilePhoto}?t=${Date.now()}`);
    }
    if (user?.isFollowedByLoggedInUser)
      setIsFollowing(user.isFollowedByLoggedInUser);
  }, [user]);

  const handleFollow = async () => {
    if (user) {
      const followed = await followUser(user.id);
      setIsFollowing(followed);
      setUser((prev) =>
        prev
          ? {
              ...prev,
              followersCount: prev.followersCount + 1,
            }
          : prev
      );
    }
  };

  const handleUnfollow = async () => {
    if (user) {
      await unfollowUser(user.id);
      setIsFollowing(false);
      setUser((prev) =>
        prev
          ? {
              ...prev,
              followersCount: prev.followersCount - 1,
            }
          : prev
      );
    }
  };

  return (
    <div className="profile-info">
      <img
        src={imgSrc}
        alt=""
        className="profile-picture"
        onError={() => setImgSrc(ARTIST_FALLBACK_IMAGE)}
      />
      <h1 className="profile-name">
        {loading ? (
          <span
            className="skeleton skeleton-text"
            style={{ width: "7rem", height: "1.2rem" }}
          />
        ) : (
          user?.name
        )}
      </h1>
      <p className="profile-username">
        {loading ? (
          <span
            className="skeleton skeleton-text"
            style={{ width: "7rem", height: "1.2rem" }}
          />
        ) : (
          `@${user?.userName}`
        )}
      </p>

      <div className="profile-following">
        <p
          className="profile-follow-count"
          onClick={() =>
            setIsFollowersModalOpen({ isOpen: true, tab: "followers" })
          }
        >
          <span>
            {loading ? (
              <span
                className="skeleton skeleton-text"
                style={{ width: "3rem", height: "1.2rem" }}
              />
            ) : (
              formatFollowCount(user?.followersCount)
            )}
          </span>{" "}
          {t("profile.followersLabel")}
        </p>

        <p
          className="profile-follow-count"
          onClick={() =>
            setIsFollowersModalOpen({ isOpen: true, tab: "following" })
          }
        >
          <span>
            {loading ? (
              <span
                className="skeleton skeleton-text"
                style={{ width: "3rem", height: "1.2rem" }}
              />
            ) : (
              formatFollowCount(user?.followingCount)
            )}
          </span>{" "}
          {t("profile.followingLabel")}
        </p>
      </div>

      <div className="profile-follow-btns">
        {isMyProfile ? (
          <button
            className="profile-follow-btn"
            onClick={() => setIsSettingsOpen(true)}
          >
            {t("profile.editProfile")}
          </button>
        ) : isFollowing ? (
          <button className="profile-follow-btn" onClick={handleUnfollow}>
            {t("profile.unfollow")}
          </button>
        ) : (
          <button className="profile-follow-btn" onClick={handleFollow}>
            {t("profile.follow")}
          </button>
        )}
        {!isMyProfile && (
          <MdOutlineChatBubble
            title={t("chat.sendMessage")}
            onClick={() =>
              navigate("/chat", {
                state: {
                  selectedUser: {
                    userId: user?.id,
                    name: user?.name,
                    userName: user?.userName,
                    profilePhoto: user?.profilePhoto,
                  },
                },
              })
            }
          />
        )}
      </div>

      {isMyProfile && (
        <div
          className="profile-settings"
          title={t("profile.settingsTitle")}
          onClick={() => setIsSettingsOpen((prev) => !prev)}
        >
          <IoSettingsOutline />
        </div>
      )}

      {isSettingsOpen && user && (
        <SettingsModal
          user={user}
          triggerRefetchUser={triggerRefetchUser}
          onClose={() => setIsSettingsOpen(false)}
        />
      )}

      {isFollowersModalOpen.isOpen && user && (
        <FollowersModal
          userId={user.id}
          tab={isFollowersModalOpen.tab}
          onClose={() => setIsFollowersModalOpen({ isOpen: false })}
        />
      )}
    </div>
  );
};

export default UserInfo;
