import { useRef, useState } from "react";
import { useTranslation } from "react-i18next";
import DiscoverNavbar from "../components/DiscoverNavbar";
import Dock from "../components/Dock";
import ActiveAuctionFeedCard from "../components/discover-page/ActiveAuctionFeedCard";
import MadeOfferFeedCard from "../components/discover-page/MadeOfferFeedCard";
import { useActiveAuctions } from "../hooks/useActiveAuctions";
import { useMyOffers } from "../hooks/useMyOffers";
import { useOnSaleArtworks } from "../hooks/useOnSaleArtworks";
import { useScroll } from "../hooks/useScroll";
import "../styles/ActiveAuctionsPage.css";

type ActiveAuctionsTab = "auctions" | "fixed" | "offers";

const ActiveAuctionsPage = () => {
  const { t } = useTranslation();
  const [activeTab, setActiveTab] = useState<ActiveAuctionsTab>("auctions");
  const [openOfferId, setOpenOfferId] = useState<number | null>(null);

  const { auctions, loadingAuctions, loadMoreAuctions } = useActiveAuctions();
  const {
    artworks: fixedPriceArtworks,
    loadingArtworks: loadingFixedPriceArtworks,
    loadMoreArtworks: loadMoreFixedPriceArtworks,
  } = useOnSaleArtworks();
  const { offers, loadingOffers, loadMoreOffers, withdraw } = useMyOffers();

  const activeAuctionsPageRef = useRef<HTMLDivElement>(null);

  useScroll({
    ref: activeAuctionsPageRef,
    storageKey: "activeAuctionsScrollY",
    onReachBottom: () => {
      if (activeTab === "auctions") loadMoreAuctions();
      else if (activeTab === "fixed") loadMoreFixedPriceArtworks();
      else if (activeTab === "offers") loadMoreOffers();
    },
  });

  return (
    <div className="fixed-page">
      <div className="active-auctions-page" ref={activeAuctionsPageRef}>
        <DiscoverNavbar />
        <h1>{t("auctions.pageTitle")}</h1>

        <div className="aap-container">
          <div className="aap-sidebar">
            <button
              className={`aap-sidebar-btn${
                activeTab === "auctions" ? " active" : ""
              }`}
              onClick={() => setActiveTab("auctions")}
            >
              {t("auctions.activeAuctionsTab")}
            </button>
            <button
              className={`aap-sidebar-btn${
                activeTab === "fixed" ? " active" : ""
              }`}
              onClick={() => setActiveTab("fixed")}
            >
              {t("auctions.fixedPrice")}
            </button>
            <button
              className={`aap-sidebar-btn${
                activeTab === "offers" ? " active" : ""
              }`}
              onClick={() => setActiveTab("offers")}
            >
              {t("auctions.madeOffersTab")}
            </button>
          </div>

          <div className="aap-content">
            {activeTab === "auctions" &&
              (!loadingAuctions && auctions.length === 0 ? (
                <p className="aap-no-results">
                  {t("auctions.noActiveAuctionsFound")}
                </p>
              ) : (
                <div className="aap-auctions-grid">
                  {auctions.map((auction) => (
                    <ActiveAuctionFeedCard
                      listing={{ ...auction, type: "auction" }}
                      key={`auction-${auction.auctionId}`}
                    />
                  ))}

                  {loadingAuctions && <div className="loading-spinner" />}
                </div>
              ))}

            {activeTab === "fixed" &&
              (!loadingFixedPriceArtworks && fixedPriceArtworks.length === 0 ? (
                <p className="aap-no-results">
                  {t("auctions.noArtworksForSaleFound")}
                </p>
              ) : (
                <div className="aap-auctions-grid">
                  {fixedPriceArtworks.map((artwork) => (
                    <ActiveAuctionFeedCard
                      listing={{ ...artwork, type: "fixed" }}
                      key={`fixed-${artwork.id}`}
                    />
                  ))}

                  {loadingFixedPriceArtworks && (
                    <div className="loading-spinner" />
                  )}
                </div>
              ))}

            {activeTab === "offers" &&
              (!loadingOffers && offers.length === 0 ? (
                <p className="aap-no-results">
                  {t("auctions.noOffersMade")}
                </p>
              ) : (
                <div className="aap-offers-list">
                  {offers.map((offer) => (
                    <MadeOfferFeedCard
                      offer={offer}
                      key={offer.id}
                      isMenuOpen={openOfferId === offer.id}
                      onOpenChange={(open) =>
                        setOpenOfferId(open ? offer.id : null)
                      }
                      onWithdraw={withdraw}
                    />
                  ))}

                  {loadingOffers && <div className="loading-spinner" />}
                </div>
              ))}
          </div>
        </div>
        <Dock />
      </div>
    </div>
  );
};

export default ActiveAuctionsPage;
