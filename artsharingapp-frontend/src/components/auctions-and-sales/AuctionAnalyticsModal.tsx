import { useTranslation } from "react-i18next";
import { useAuctionContext } from "../../context/AuctionContext";
import { useOffers } from "../../hooks/useOffers";
import "./styles/AuctionAnalyticsModal.css";
import { IoCloseCircleOutline } from "react-icons/io5";
import OfferCard from "./OfferCard";
import { useState } from "react";

type AuctionAnalyticsModalProps = {
  onClose: () => void;
};

const AuctionAnalyticsModal = ({ onClose }: AuctionAnalyticsModalProps) => {
  const { t } = useTranslation();
  const [refetchOffers, setRefetchOffers] = useState<boolean>(false);

  const { auction } = useAuctionContext();

  const { offers } = useOffers(auction?.id ? auction.id : -1, refetchOffers);

  return (
    <div className="blured-page">
      <div className="modal-container aam-container">
        <IoCloseCircleOutline
          className="aam-close"
          onClick={onClose}
          title={t("common.closeTitle")}
        />

        <h4>{t("artwork.auctionAnalytics")}</h4>

        <div className="aam-content">
          <div className="aam-sidebar">
            {auction ? (
              <>
                <p className="aam-sidebar-section">{t("auctions.activeLabel")}</p>
                <div className="aam-auction">
                  <p className="aam-auction-time">
                    {new Date(auction.startTime).toLocaleString("en-GB", {
                      year: "numeric",
                      month: "numeric",
                      day: "numeric",
                      hour: "2-digit",
                      minute: "2-digit",
                    })}
                  </p>
                  <span className="aam-arrow">→</span>
                  <p className="aam-auction-time">
                    {new Date(auction.endTime).toLocaleString("en-GB", {
                      year: "numeric",
                      month: "numeric",
                      day: "numeric",
                      hour: "2-digit",
                      minute: "2-digit",
                    })}
                  </p>
                </div>
              </>
            ) : (
              <p className="aam-auction-not-found">
                {t("auctions.noActiveAuctionForArtwork")}
              </p>
            )}
          </div>

          {offers && offers.length !== 0 && (
            <div className="aam-offers">
              {offers.map((offer) => (
                <OfferCard
                  key={offer.id}
                  offer={offer}
                  currency={auction?.currency}
                  onClose={onClose}
                  refetchOffers={() => setRefetchOffers((prev) => !prev)}
                />
              ))}
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default AuctionAnalyticsModal;
