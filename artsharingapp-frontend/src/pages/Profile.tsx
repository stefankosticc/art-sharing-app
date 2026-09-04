import { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import "../styles/Profile.css";
import { useLoggedInUser } from "../hooks/useLoggedInUser";
import Dock from "../components/Dock";
import TextEditor from "../components/TextEditor";
import { MdEdit } from "react-icons/md";
import { getUserByUsername, updateUserBiography } from "../services/user";
import UserInfo from "../components/profile/UserInfo";
import MyArtworksGrid from "../components/profile/MyArtworksGrid";
import FavoriteArtworksGrid from "../components/profile/FavoriteArtworksGrid";
import { useParams } from "react-router-dom";
import NotFound from "./NotFound";
import { User } from "../services/auth";
import Loading from "./Loading";

const TABS: {
  key: string;
  labelKey: string;
}[] = [
  { key: "artworks", labelKey: "profile.tabs.artworks" },
  { key: "favorites", labelKey: "profile.tabs.favorites" },
  { key: "biography", labelKey: "profile.tabs.biography" },
];

const Profile = () => {
  const { t } = useTranslation();
  const { username } = useParams();
  const [activeTab, setActiveTab] = useState<string>("artworks");
  const [isEditingBiography, setIsEditingBiography] = useState<boolean>(false);
  const [biography, setBiography] = useState<string>("");
  const [profileUser, setProfileUser] = useState<User | null>(null);
  const [loadingProfileUser, setLoadingProfileUser] = useState<boolean>(true);
  const [error, setError] = useState<unknown>(null);
  const [refetchUser, setRefetchUser] = useState(0);

  const { loggedInUser, loading: loadingLoggedInUser } = useLoggedInUser();

  useEffect(() => {
    const fetchProfileUser = async () => {
      if (!username) {
        setError("No username provided");
        setLoadingProfileUser(false);
        return;
      }
      try {
        setLoadingProfileUser(true);
        const user = await getUserByUsername(username);
        if (!user) {
          setError("User not found");
          setProfileUser(null);
        } else {
          setProfileUser(user);
          setBiography(user?.biography || "");
        }
      } catch (err) {
        setError(err);
        setProfileUser(null);
      } finally {
        setLoadingProfileUser(false);
      }
    };

    fetchProfileUser();
  }, [username, refetchUser]);

  const triggerRefetchUser = () => setRefetchUser((prev) => prev + 1);

  // Determine if the profile belongs to the logged-in user
  const isMyProfile = loggedInUser?.userName === username;

  if (!loadingProfileUser && (!profileUser || error)) {
    return <NotFound />;
  }

  if (loadingProfileUser || loadingProfileUser) {
    return <Loading />;
  }

  return (
    <div className="profile-page page">
      {/* USER INFO */}
      <UserInfo
        user={profileUser}
        setUser={setProfileUser}
        triggerRefetchUser={triggerRefetchUser}
        loading={loadingProfileUser}
        loggedInUser={loggedInUser}
        isMyProfile={isMyProfile}
      />

      <div className="profile-divider"></div>

      {/* TABS */}
      <div className="profile-content-container">
        <div className="profile-tabs">
          {TABS.map((tab) => (
            <button
              key={tab.key}
              className={`profile-tab${activeTab === tab.key ? " active" : ""}`}
              onClick={() => setActiveTab(tab.key)}
              type="button"
              title={t("profile.tabTitle", { label: t(tab.labelKey) })}
            >
              {t(tab.labelKey)}
            </button>
          ))}
        </div>

        {/* MY ARTWORKS */}
        <MyArtworksGrid
          activeTab={activeTab}
          user={profileUser}
          isMyProfile={isMyProfile}
        />

        {/* FAVORITES */}
        <FavoriteArtworksGrid activeTab={activeTab} user={profileUser} />

        {/* BIOGRAPHY */}
        <div
          className={`profile-content${
            activeTab === "biography" ? " active" : ""
          }`}
        >
          {(biography !== "" && biography !== "<p></p>") ||
          isEditingBiography ? (
            <div className="biography-container">
              <TextEditor
                content={biography}
                editable={isEditingBiography && isMyProfile}
                className={`profile-content-biography ${
                  isEditingBiography ? "text-editor-editing" : ""
                }`}
                onUpdate={({ editor }) => {
                  if (isEditingBiography) setBiography(editor.getHTML());
                }}
              />
              {isMyProfile && (
                <MdEdit
                  className="biography-edit-icon"
                  onClick={() => setIsEditingBiography(!isEditingBiography)}
                  title={t("common.edit")}
                />
              )}
            </div>
          ) : isMyProfile ? (
            <p className="profile-content-text not-found">
              {t("profile.biographyEmptyOwn", {
                editLabel: t("common.edit"),
              })}
              <br />
              <button
                className="biography-editing-button"
                onClick={() => setIsEditingBiography(true)}
              >
                {t("common.edit")}
              </button>
            </p>
          ) : (
            <p className="profile-content-text not-found">
              {t("profile.biographyEmptyOther")}
            </p>
          )}
          {isEditingBiography && isMyProfile && (
            <div className="biography-buttons-container">
              <button
                className="biography-editing-button"
                id="biography-cancel-button"
                onClick={() => {
                  setBiography(profileUser?.biography || "");
                  setIsEditingBiography(false);
                }}
                title={t("common.cancel")}
              >
                {t("common.cancel")}
              </button>
              <button
                className="biography-editing-button"
                id="biography-save-button"
                onClick={async () => {
                  await updateUserBiography({ biography });
                  setIsEditingBiography(false);
                }}
                title={t("profile.saveChangesTitle")}
              >
                {t("common.save")}
              </button>
            </div>
          )}
        </div>
      </div>

      <Dock />
    </div>
  );
};

export default Profile;
