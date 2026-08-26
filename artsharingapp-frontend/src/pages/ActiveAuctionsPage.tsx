import { useRef, useState } from "react";
import DiscoverNavbar from "../components/DiscoverNavbar";
import Dock from "../components/Dock";
import ActiveAuctionFeedCard from "../components/discover-page/ActiveAuctionFeedCard";
import MadeOfferFeedCard, {
  MockOffer,
} from "../components/discover-page/MadeOfferFeedCard";
import { useActiveAuctions } from "../hooks/useActiveAuctions";
import { useOnSaleArtworks } from "../hooks/useOnSaleArtworks";
import { useScroll } from "../hooks/useScroll";
import { Currency, OfferStatus } from "../services/enums";
import "../styles/ActiveAuctionsPage.css";

type ActiveAuctionsTab = "auctions" | "fixed" | "offers";

const MOCK_OFFERS: MockOffer[] = [
  {
    id: 1,
    auctionId: 3,
    artworkId: 103,
    artworkTitle: "Midnight Orchard",
    artworkImage: "https://picsum.photos/seed/auction3/600/450",
    artistUserName: "sofia.ink",
    amount: 9600,
    currency: Currency.USD,
    status: OfferStatus.SUBMITTED,
    auctionEndTime: new Date(Date.now() + 1000 * 60 * 3),
  },
  {
    id: 2,
    auctionId: 6,
    artworkId: 106,
    artworkTitle: "Static Bloom",
    artworkImage: "https://picsum.photos/seed/offer2/600/450",
    artistUserName: "ren.oda",
    amount: 1500,
    currency: Currency.USD,
    status: OfferStatus.ACCEPTED,
    auctionEndTime: new Date(Date.now() - 1000 * 60 * 60 * 3),
  },
  {
    id: 3,
    auctionId: 7,
    artworkId: 107,
    artworkTitle: "Hollow Gold",
    artworkImage: "https://picsum.photos/seed/offer3/600/450",
    artistUserName: "priya.d",
    amount: 640,
    currency: Currency.GBP,
    status: OfferStatus.REJECTED,
    auctionEndTime: new Date(Date.now() - 1000 * 60 * 60 * 20),
  },
  {
    id: 4,
    auctionId: 2,
    artworkId: 102,
    artworkTitle: "Fractured Light",
    artworkImage: "https://picsum.photos/seed/auction2/600/450",
    artistUserName: "kwabena.art",
    amount: 1700,
    currency: Currency.EUR,
    status: OfferStatus.WITHDRAWN,
    auctionEndTime: new Date(Date.now() + 1000 * 60 * 60 * 5),
  },
];

const ActiveAuctionsPage = () => {
  const [activeTab, setActiveTab] = useState<ActiveAuctionsTab>("auctions");
  const [openOfferId, setOpenOfferId] = useState<number | null>(null);

  const { auctions, loadingAuctions, loadMoreAuctions } = useActiveAuctions();
  const {
    artworks: fixedPriceArtworks,
    loadingArtworks: loadingFixedPriceArtworks,
    loadMoreArtworks: loadMoreFixedPriceArtworks,
  } = useOnSaleArtworks();

  const activeAuctionsPageRef = useRef<HTMLDivElement>(null);

  useScroll({
    ref: activeAuctionsPageRef,
    storageKey: "activeAuctionsScrollY",
    onReachBottom: () => {
      if (activeTab === "auctions") loadMoreAuctions();
      else if (activeTab === "fixed") loadMoreFixedPriceArtworks();
    },
  });

  return (
    <div className="fixed-page">
      <div className="active-auctions-page" ref={activeAuctionsPageRef}>
        <DiscoverNavbar />
        <h1>Auctions</h1>

        <div className="aap-container">
          <div className="aap-sidebar">
            <button
              className={`aap-sidebar-btn${
                activeTab === "auctions" ? " active" : ""
              }`}
              onClick={() => setActiveTab("auctions")}
            >
              Active Auctions
            </button>
            <button
              className={`aap-sidebar-btn${
                activeTab === "fixed" ? " active" : ""
              }`}
              onClick={() => setActiveTab("fixed")}
            >
              Fixed Price
            </button>
            <button
              className={`aap-sidebar-btn${
                activeTab === "offers" ? " active" : ""
              }`}
              onClick={() => setActiveTab("offers")}
            >
              Made Offers
            </button>
          </div>

          <div className="aap-content">
            {activeTab === "auctions" &&
              (!loadingAuctions && auctions.length === 0 ? (
                <p className="aap-no-results">No active auctions found.</p>
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
                  No artworks for sale found.
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
              (MOCK_OFFERS.length === 0 ? (
                <p className="aap-no-results">
                  You haven't made any offers yet.
                </p>
              ) : (
                <div className="aap-offers-list">
                  {MOCK_OFFERS.map((offer) => (
                    <MadeOfferFeedCard
                      offer={offer}
                      key={offer.id}
                      isMenuOpen={openOfferId === offer.id}
                      onOpenChange={(open) =>
                        setOpenOfferId(open ? offer.id : null)
                      }
                    />
                  ))}
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
