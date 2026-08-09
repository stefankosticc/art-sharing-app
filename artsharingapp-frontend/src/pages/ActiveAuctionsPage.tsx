import { useState } from "react";
import DiscoverNavbar from "../components/DiscoverNavbar";
import Dock from "../components/Dock";
import ActiveAuctionFeedCard, {
  MockAuctionListing,
  MockFixedSaleListing,
  MockListing,
} from "../components/discover-page/ActiveAuctionFeedCard";
import MadeOfferFeedCard, {
  MockOffer,
} from "../components/discover-page/MadeOfferFeedCard";
import { Currency, OfferStatus } from "../services/enums";
import "../styles/ActiveAuctionsPage.css";

type ActiveAuctionsTab = "auctions" | "offers";

const MOCK_AUCTIONS: MockAuctionListing[] = [
  {
    type: "auction",
    id: 1,
    artworkId: 101,
    artworkTitle: "Whispers of the Coast",
    artworkImage: "https://picsum.photos/seed/auction1/600/450",
    artistUserName: "elena.marin",
    currentPrice: 4200,
    offerCount: 12,
    currency: Currency.USD,
    endTime: new Date(Date.now() + 1000 * 60 * 47),
  },
  {
    type: "auction",
    id: 2,
    artworkId: 102,
    artworkTitle: "Fractured Light",
    artworkImage: "https://picsum.photos/seed/auction2/600/450",
    artistUserName: "kwabena.art",
    currentPrice: 1850,
    offerCount: 4,
    currency: Currency.EUR,
    endTime: new Date(Date.now() + 1000 * 60 * 60 * 5),
  },
  {
    type: "auction",
    id: 3,
    artworkId: 103,
    artworkTitle: "Midnight Orchard",
    artworkImage: "https://picsum.photos/seed/auction3/600/450",
    artistUserName: "sofia.ink",
    currentPrice: 9600,
    offerCount: 27,
    currency: Currency.USD,
    endTime: new Date(Date.now() + 1000 * 60 * 3),
  },
  {
    type: "auction",
    id: 4,
    artworkId: 104,
    artworkTitle: "Glasswing",
    artworkImage: "https://picsum.photos/seed/auction4/600/450",
    artistUserName: "marcus.lee",
    currentPrice: 720,
    offerCount: 2,
    currency: Currency.GBP,
    endTime: new Date(Date.now() + 1000 * 60 * 60 * 26),
  },
  {
    type: "auction",
    id: 5,
    artworkId: 105,
    artworkTitle: "Paper Moon",
    artworkImage: "https://picsum.photos/seed/auction5/600/450",
    artistUserName: "nadia.k",
    currentPrice: 3100,
    offerCount: 9,
    currency: Currency.USD,
    endTime: new Date(Date.now() + 1000 * 60 * 60 * 2),
  },
];

const MOCK_FIXED_SALES: MockFixedSaleListing[] = [
  {
    type: "fixed",
    id: 6,
    artworkId: 106,
    artworkTitle: "Static Bloom",
    artworkImage: "https://picsum.photos/seed/fixed1/600/450",
    artistUserName: "ren.oda",
    price: 1500,
    currency: Currency.USD,
  },
  {
    type: "fixed",
    id: 7,
    artworkId: 107,
    artworkTitle: "Hollow Gold",
    artworkImage: "https://picsum.photos/seed/fixed2/600/450",
    artistUserName: "priya.d",
    price: 640,
    currency: Currency.GBP,
  },
  {
    type: "fixed",
    id: 8,
    artworkId: 108,
    artworkTitle: "Cobalt Drift",
    artworkImage: "https://picsum.photos/seed/fixed3/600/450",
    artistUserName: "yusuf.a",
    price: 2300,
    currency: Currency.EUR,
  },
];

const MOCK_LISTINGS: MockListing[] = [...MOCK_AUCTIONS, ...MOCK_FIXED_SALES];

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

  return (
    <div className="fixed-page">
      <div className="active-auctions-page">
        <DiscoverNavbar />
        <h1>Active Auctions</h1>

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
                activeTab === "offers" ? " active" : ""
              }`}
              onClick={() => setActiveTab("offers")}
            >
              Made Offers
            </button>
          </div>

          <div className="aap-content">
            {activeTab === "auctions" &&
              (MOCK_LISTINGS.length === 0 ? (
                <p className="aap-no-results">No active auctions found.</p>
              ) : (
                <div className="aap-auctions-grid">
                  {MOCK_LISTINGS.map((listing) => (
                    <ActiveAuctionFeedCard
                      listing={listing}
                      key={`${listing.type}-${listing.id}`}
                    />
                  ))}
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
