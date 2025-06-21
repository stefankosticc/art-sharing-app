import { useEffect, useState } from "react";
import "../styles/Profile.css";
import { useLoggedInUser } from "../hooks/useLoggedInUser";
import { formatFollowCount } from "../utils/formatting";
import ArtworkCard from "../components/ArtworkCard";
import { ArtworkCardData, getMyArtworks } from "../services/artwork";
import { useFavoriteArtworks } from "../hooks/useFavoriteArtworks";
import Dock from "../components/Dock";

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

  const { loggedInUser, loading, error } = useLoggedInUser();

  useEffect(() => {
    const fetchArtworks = async () => {
      try {
        const response = await getMyArtworks();
        setArtworks(response);
      } catch (err) {
        setLoadingArtwork(true);
      } finally {
        setTimeout(() => {
          setLoadingArtwork(false);
        }, 2000); //TODO: Simulate a delay for loading state DELETE THIS LATER
      }
    };

    fetchArtworks();
  }, []);

  const { favorites, loadingFavorites } = useFavoriteArtworks({
    likedByUser: loggedInUser,
    activeTab,
  });

  return (
    <div className="profile-page page">
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

      {/* TABS */}
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
          {loggedInUser?.biography ? (
            <p className="profile-content-biography">
              {loggedInUser.biography}
            </p>
          ) : (
            <p className="profile-content-text not-found">
              You haven't added a biography yet. <br />
              You can add your biography here.
            </p>
          )}
        </div>
      </div>

      <Dock />
    </div>
  );
};

export default Profile;
