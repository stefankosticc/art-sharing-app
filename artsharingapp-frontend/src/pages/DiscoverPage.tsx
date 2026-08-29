import { useEffect, useRef, useState } from "react";
import { useTranslation } from "react-i18next";
import Dock from "../components/Dock";
import TopArtistsSection from "../components/discover-page/TopArtistsSection";
import { useScroll } from "../hooks/useScroll";
import "../styles/DiscoverPage.css";
import { useDiscoverArtworks } from "../hooks/useDiscoverArtworks";
import ArtworkFeedCard from "../components/discover-page/ArtworkFeedCard";
import { DiscoverData, getDiscoverData } from "../services/discover";
import HighStakesAuctionsSection from "../components/discover-page/HighStakesAuctionSection";
import TrendingArtworksSection from "../components/discover-page/TrendingArtworksSection";
import DiscoverNavbar from "../components/DiscoverNavbar";

const DiscoverPage = () => {
  const { t } = useTranslation();
  const [discoverData, setDiscoverData] = useState<DiscoverData | null>(null);
  const [loadingDiscoverData, setLoadingDiscoverData] =
    useState<boolean>(false);

  const { artworks, loadingArtworks, loadMoreArtworks } =
    useDiscoverArtworks("discover");

  const popularArtworksRef = useRef<HTMLDivElement>(null);

  useScroll({
    ref: popularArtworksRef,
    storageKey: "followedUsersArtworksScrollY",
    onReachBottom: loadMoreArtworks,
  });

  useEffect(() => {
    let isCancelled = false;

    const fetchDiscoverData = async () => {
      try {
        setLoadingDiscoverData(true);
        const response = await getDiscoverData();
        if (!isCancelled) {
          setDiscoverData(response);
        }
      } catch (err) {
        if (!isCancelled) {
          console.error("Failed to fetch discover data.");
          setDiscoverData(null);
        }
      } finally {
        if (!isCancelled) {
          setLoadingDiscoverData(false);
        }
      }
    };

    fetchDiscoverData();

    return () => {
      isCancelled = true;
    };
  }, []);

  return (
    <div className="fixed-page">
      <div className="discover-page">
        <DiscoverNavbar />

        {loadingDiscoverData ? (
          <div className="loading-discover-data">
            <div className="discover-loader" />
          </div>
        ) : (
          <>
            {discoverData?.topArtistsByLikes && (
              <>
                <h1>🧑‍🎨 {t("discover.topArtists")}</h1>
                <TopArtistsSection artists={discoverData.topArtistsByLikes} />
              </>
            )}

            {discoverData?.highStakeAuctions && (
              <>
                <h2>🔥 {t("discover.highStakesAuctions")}</h2>
                <HighStakesAuctionsSection
                  auctions={discoverData.highStakeAuctions}
                />
              </>
            )}

            {discoverData?.trendingArtworks && (
              <>
                <h2>✨ {t("discover.onTheRise")}</h2>
                <TrendingArtworksSection
                  artworks={discoverData.trendingArtworks}
                />
              </>
            )}

            <h2>{t("discover.freshFinds")}</h2>
            <div className="discover-feed">
              {artworks.map((artwork) => (
                <ArtworkFeedCard artwork={artwork} key={artwork.id} />
              ))}

              {!loadingArtworks && artworks.length === 0 && (
                <p className="discover-no-results">
                  {t("common.noArtworksFound")}
                </p>
              )}

              {loadingArtworks && <div className="discover-loader" />}
            </div>
          </>
        )}
      </div>

      <Dock />
    </div>
  );
};

export default DiscoverPage;
