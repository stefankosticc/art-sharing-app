import { Currency } from "../../services/enums";
import Countdown from "../Countdown";
import "./styles/ActiveAuctionFeedCard.css";

export type MockAuctionListing = {
  type: "auction";
  id: number;
  artworkId: number;
  artworkTitle: string;
  artworkImage: string;
  artistUserName: string;
  currentPrice: number;
  offerCount: number;
  currency: Currency;
  endTime: Date;
};

export type MockFixedSaleListing = {
  type: "fixed";
  id: number;
  artworkId: number;
  artworkTitle: string;
  artworkImage: string;
  artistUserName: string;
  price: number;
  currency: Currency;
};

export type MockListing = MockAuctionListing | MockFixedSaleListing;

type ActiveAuctionFeedCardProps = {
  listing: MockListing;
};

const ActiveAuctionFeedCard = ({ listing }: ActiveAuctionFeedCardProps) => {
  return (
    <div className="aac-container">
      <img
        src={listing.artworkImage}
        alt={listing.artworkTitle}
        className="aac-img"
      />

      <div className="aac-content">
        <div className="aac-header">
          <p className="aac-title">{listing.artworkTitle}</p>
          {listing.type === "auction" ? (
            <p className="aac-time">
              Time left: <Countdown endTime={listing.endTime} />
            </p>
          ) : (
            <p className="aac-time aac-fixed-badge">Fixed Price</p>
          )}
        </div>
        <p className="aac-artist">@{listing.artistUserName}</p>

        <hr />

        {listing.type === "auction" ? (
          <div className="aac-info">
            <div className="aac-info-column">
              <p>Current Price</p>
              <p className="aac-info-value">
                {listing.currentPrice.toLocaleString("en-US")}{" "}
                {Currency[listing.currency]}
              </p>
            </div>
            <div className="aac-info-column">
              <p>No. of Offers</p>
              <p className="aac-info-value">{listing.offerCount}</p>
            </div>
          </div>
        ) : (
          <div className="aac-info">
            <div className="aac-info-column">
              <p>Price</p>
              <p className="aac-info-value">
                {listing.price.toLocaleString("en-US")}{" "}
                {Currency[listing.currency]}
              </p>
            </div>
            <div className="aac-info-column"></div>
          </div>
        )}
      </div>
    </div>
  );
};

export default ActiveAuctionFeedCard;
