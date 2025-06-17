import { useState } from "react";
import "../styles/Profile.css";
import { useLoggedInUser } from "../hooks/useLoggedInUser";
import { formatFollowCount } from "../utils/formatting";

const TABS: {
  key: string;
  label: string;
}[] = [
  { key: "artworks", label: "Artworks" },
  { key: "favorites", label: "Favorites" },
  { key: "biography", label: "Biography" },
];

const Profile = () => {
  const [activeTab, setActiveTab] = useState<string>("artworks");

  const { loggedInUser, loading, error } = useLoggedInUser();

  return (
    <div className="profile-page">
      <div className="profile-info">
        <img
          src="https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png"
          alt="Default profile picture"
          className="profile-picture"
        />
        <h1 className="profile-name">
          {loading ? (
            <span
              className="skeleton skeleton-text"
              style={{ width: "7rem", height: "1.2rem" }}
            />
          ) : (
            loggedInUser?.name
          )}
        </h1>
        <p className="profile-username">
          {loading ? (
            <span
              className="skeleton skeleton-text"
              style={{ width: "7rem", height: "1.2rem" }}
            />
          ) : (
            `@${loggedInUser?.userName}`
          )}
        </p>

        <div className="profile-following">
          <p className="profile-follow-count">
            <span>
              {loading ? (
                <span
                  className="skeleton skeleton-text"
                  style={{ width: "3rem", height: "1.2rem" }}
                />
              ) : (
                formatFollowCount(loggedInUser?.followersCount)
              )}
            </span>{" "}
            Followers
          </p>
          <p className="profile-follow-count">
            <span>
              {loading ? (
                <span
                  className="skeleton skeleton-text"
                  style={{ width: "3rem", height: "1.2rem" }}
                />
              ) : (
                formatFollowCount(loggedInUser?.followingCount)
              )}
            </span>{" "}
            Following
          </p>
        </div>
      </div>

      <div className="profile-divider"></div>

      <div className="profile-content-container">
        <div className="profile-tabs">
          {TABS.map((tab) => (
            <button
              key={tab.key}
              className={`profile-tab${activeTab === tab.key ? " active" : ""}`}
              onClick={() => setActiveTab(tab.key)}
              type="button"
            >
              {tab.label}
            </button>
          ))}
        </div>

        <div
          className={`profile-content${
            activeTab === "artworks" ? " active" : ""
          }`}
        >
          <p className="profile-content-text not-found">
            You have no artworks yet.
          </p>
        </div>

        <div
          className={`profile-content${
            activeTab === "favorites" ? " active" : ""
          }`}
        >
          <p className="profile-content-text not-found">
            You have no favorites yet.
          </p>
        </div>

        <div
          className={`profile-content${
            activeTab === "biography" ? " active" : ""
          }`}
        >
          <p className="profile-content-text not-found">
            You haven't added a biography yet. <br />
            You can add your biography here.
          </p>
        </div>
      </div>
    </div>
  );
};

export default Profile;
