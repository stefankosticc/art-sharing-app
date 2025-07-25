import { useEffect, useState } from "react";
import "../styles/Profile.css";
import { useLoggedInUser } from "../hooks/useLoggedInUser";
import { formatFollowCount } from "../utils/formatting";
import ArtworkCard from "../components/ArtworkCard";
import { ArtworkCardData, getMyArtworks } from "../services/artwork";
import { useFavoriteArtworks } from "../hooks/useFavoriteArtworks";
import Dock from "../components/Dock";
import TextEditor from "../components/TextEditor";
import { MdEdit } from "react-icons/md";
import { updateUserBiography } from "../services/user";
import { ARTIST_FALLBACK_IMAGE, BACKEND_BASE_URL } from "../config/constants";

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
  const [artworks, setArtworks] = useState<ArtworkCardData[] | null>(null);
  const [loadingArtwork, setLoadingArtwork] = useState<boolean>(true);
  const [isEditingBiography, setIsEditingBiography] = useState<boolean>(false);
  const [biography, setBiography] = useState<string>("");
  const [imgSrc, setImgSrc] = useState<string>("");

  const { loggedInUser, loading, error } = useLoggedInUser();

  useEffect(() => {
    const fetchArtworks = async () => {
      try {
        const response = await getMyArtworks();
        setArtworks(response);
      } catch (err) {
        setLoadingArtwork(true);
      } finally {
        setLoadingArtwork(false);
      }
    };

    fetchArtworks();
  }, []);

  useEffect(() => {
    if (loggedInUser?.profilePhoto)
      setImgSrc(
        `${BACKEND_BASE_URL}${loggedInUser.profilePhoto}?t=${Date.now()}`
      );
    setBiography(loggedInUser?.biography || "");
  }, [loggedInUser]);

  const { favorites, loadingFavorites } = useFavoriteArtworks({
    likedByUser: loggedInUser,
    activeTab,
  });

  return (
    <div className="profile-page page">
      <div className="profile-info">
        <img
          src={imgSrc ? imgSrc : ARTIST_FALLBACK_IMAGE}
          alt="Profile picture"
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

      {/* TABS */}
      <div className="profile-content-container">
        <div className="profile-tabs">
          {TABS.map((tab) => (
            <button
              key={tab.key}
              className={`profile-tab${activeTab === tab.key ? " active" : ""}`}
              onClick={() => setActiveTab(tab.key)}
              type="button"
              title={tab.label + " tab"}
            >
              {tab.label}
            </button>
          ))}
        </div>

        {/* MY ARTWORKS */}
        <div
          className={`profile-content${
            activeTab === "artworks" ? " active" : ""
          }`}
        >
          {artworks && artworks.length !== 0 ? (
            <div className="profile-artwork-grid">
              {artworks.map((artwork) => (
                <ArtworkCard
                  key={artwork.id}
                  artwork={artwork}
                  loading={loadingArtwork}
                />
              ))}
            </div>
          ) : (
            <p className="profile-content-text not-found">
              You have no artworks yet.
            </p>
          )}
        </div>

        {/* FAVORITES */}
        <div
          className={`profile-content${
            activeTab === "favorites" ? " active" : ""
          }`}
        >
          {favorites && favorites.length !== 0 ? (
            <div className="profile-artwork-grid">
              {favorites.map((favorite) => (
                <ArtworkCard
                  key={favorite.artworkId}
                  artwork={favorite}
                  loading={loadingFavorites}
                />
              ))}
            </div>
          ) : (
            <p className="profile-content-text not-found">
              You have no favorites yet.
            </p>
          )}
        </div>

        {/* BIOPGRAPHY */}
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
                editable={isEditingBiography}
                className={`profile-content-biography ${
                  isEditingBiography ? "text-editor-editing" : ""
                }`}
                onUpdate={({ editor }) => {
                  if (isEditingBiography) setBiography(editor.getHTML());
                }}
              />
              <MdEdit
                className="biography-edit-icon"
                onClick={() => setIsEditingBiography(!isEditingBiography)}
                title="Edit"
              />
            </div>
          ) : (
            <p className="profile-content-text not-found">
              You haven't added a biography yet. <br />
              To add one, click the "Edit" button below. <br />
              <button
                className="biography-editing-button"
                onClick={() => setIsEditingBiography(true)}
              >
                Edit
              </button>
            </p>
          )}
          {isEditingBiography && (
            <div className="biography-buttons-container">
              <button
                className="biography-editing-button"
                id="biography-cancel-button"
                onClick={() => {
                  setBiography(loggedInUser?.biography || "");
                  setIsEditingBiography(false);
                }}
                title="Cancel"
              >
                Cancel
              </button>
              <button
                className="biography-editing-button"
                id="biography-save-button"
                onClick={async () => {
                  await updateUserBiography({ biography });
                  setIsEditingBiography(false);
                }}
                title="Save changes"
              >
                Save
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
