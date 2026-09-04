import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";
import {
  ARTWORK_FALLBACK_IMAGE,
  IMAGE_SERVICE_BASE_URL,
} from "../../config/constants";
import { ActiveAuctionResponse } from "../../services/auction";
import { OnSaleArtworkResponse } from "../../services/artwork";
import { Currency } from "../../services/enums";
import Countdown from "../Countdown";
import "./styles/ActiveAuctionFeedCard.css";

export type AuctionListing = ActiveAuctionResponse & { type: "auction" };

export type FixedPriceListing = OnSaleArtworkResponse & { type: "fixed" };

export type Listing = AuctionListing | FixedPriceListing;

type ActiveAuctionFeedCardProps = {
  listing: Listing;
};

const ActiveAuctionFeedCard = ({ listing }: ActiveAuctionFeedCardProps) => {
  const { t } = useTranslation();
  const navigate = useNavigate();

  const imagePath =
    listing.type === "auction" ? listing.artworkImage : listing.image;
  const artworkTitle =
    listing.type === "auction" ? listing.artworkTitle : listing.title;
  const artworkId = listing.type === "auction" ? listing.artworkId : listing.id;

  const [imgSrc, setImgSrc] = useState<string>(
    imagePath
      ? `${IMAGE_SERVICE_BASE_URL}${imagePath}`
      : ARTWORK_FALLBACK_IMAGE,
  );

  return (
    <div
      className="aac-container"
      onClick={() => navigate(`/artwork/${artworkId}`)}
    >
      <img
        src={imgSrc}
        alt={artworkTitle}
        onError={() => setImgSrc(ARTWORK_FALLBACK_IMAGE)}
        className="aac-img"
      />

      <div className="aac-content">
        <div className="aac-header">
          <p className="aac-title">{artworkTitle}</p>
          {listing.type === "auction" ? (
            <p className="aac-time">
              {t("auctions.timeLeft")} <Countdown endTime={listing.endTime} />
            </p>
          ) : (
            <p className="aac-time aac-fixed-badge">
              {t("auctions.fixedPrice")}
            </p>
          )}
        </div>
        <p className="aac-artist">@{listing.postedByUserName}</p>

        <hr />

        {listing.type === "auction" ? (
          <div className="aac-info">
            <div className="aac-info-column">
              <p>{t("common.currentPrice")}</p>
              <p className="aac-info-value">
                {listing.currentPrice.toLocaleString("en-US")}{" "}
                {Currency[listing.currency]}
              </p>
            </div>
            <div className="aac-info-column">
              <p>{t("common.offerCount")}</p>
              <p className="aac-info-value">{listing.offerCount}</p>
            </div>
          </div>
        ) : (
          <div className="aac-info">
            <div className="aac-info-column">
              <p>{t("auctions.price")}</p>
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
