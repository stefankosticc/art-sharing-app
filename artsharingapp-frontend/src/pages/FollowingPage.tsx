import Dock from "../components/Dock";
import "../styles/FollowingPage.css";
import { useDiscoverArtworks } from "../hooks/useDiscoverArtworks";
import { useRef } from "react";
import { useTranslation } from "react-i18next";
import { useScroll } from "../hooks/useScroll";
import ArtworkFeedCard from "../components/discover-page/ArtworkFeedCard";
import DiscoverNavbar from "../components/DiscoverNavbar";

const FollowingPage = () => {
  const { t } = useTranslation();
  const { artworks, loadingArtworks, loadMoreArtworks } =
    useDiscoverArtworks("following");

  const followedUsersArtworksRef = useRef<HTMLDivElement>(null);

  useScroll({
    ref: followedUsersArtworksRef,
    storageKey: "followedUsersArtworksScrollY",
    onReachBottom: loadMoreArtworks,
  });

  return (
    <div className="fixed-page">
      <div className="following-page" ref={followedUsersArtworksRef}>
        <DiscoverNavbar />

        <h1>{t("following.pageTitle")}</h1>
        <div className="following-feed">
          {artworks.map((artwork) => (
            <ArtworkFeedCard artwork={artwork} key={artwork.id} />
          ))}

          {!loadingArtworks && artworks.length === 0 && (
            <p className="fp-no-results">{t("common.noArtworksFound")}</p>
          )}

          {loadingArtworks && <div className="fp-loader" />}
        </div>
      </div>

      <Dock />
    </div>
  );
};

export default FollowingPage;
