import { useTranslation } from "react-i18next";
import { Currency } from "../../services/enums";
import "./styles/AuctionSection.css";
import { AiOutlineSend } from "react-icons/ai";
import Countdown from "../Countdown";
import { useState } from "react";
import { makeAnOffer, OfferRequest } from "../../services/auction";
import { useAuctionContext } from "../../context/AuctionContext";
import { toast } from "react-toastify";

const AuctionSection = () => {
  const { t } = useTranslation();
  const [offerRequest, setOfferRequest] = useState<OfferRequest>({ amount: 0 });
  const { auction, triggerRefetchAuction } = useAuctionContext();

  if (!auction) return null;

  const handleSendAnOffer = async () => {
    if (
      window.confirm(
        t("auctions.confirmOffer", {
          amount: offerRequest.amount.toLocaleString("en-US"),
          currency: Currency[auction.currency],
        })
      )
    ) {
      const success = await makeAnOffer(auction.id, offerRequest);
      if (success) {
        triggerRefetchAuction();
        toast.success(
          t("auctions.offerSentSuccess", {
            amount: offerRequest.amount.toLocaleString("en-US"),
            currency: Currency[auction.currency],
          })
        );
      }
    }
  };

  return (
    <div className="auction-section-container">
      <div className="auction-section-header">
        <h4>{t("auctions.activeAuctionHeading")}</h4>
        <p
          className="auction-section-time"
          title={`${new Date(auction.startTime).toLocaleString("en-US", {
            year: "numeric",
            month: "long",
            day: "numeric",
            hour: "2-digit",
            minute: "2-digit",
          })} → ${new Date(auction.endTime).toLocaleString("en-US", {
            year: "numeric",
            month: "long",
            day: "numeric",
            hour: "2-digit",
            minute: "2-digit",
          })}`}
        >
          {t("auctions.timeLeft")} <Countdown endTime={auction.endTime} />
        </p>
      </div>
      <hr />
      <div className="auction-section-content">
        <div className="auction-section-column">
          <p>{t("common.currentPrice")}</p>
          <p className="auction-section-column-info">
            {auction.currentPrice.toLocaleString("en-US")}{" "}
            {Currency[auction.currency]}
          </p>
        </div>
        <div className="auction-section-column">
          <p>{t("common.offerCount")}</p>
          <p className="auction-section-column-info">{auction.offerCount}</p>
        </div>
        <div className="auction-section-column">
          <p>{t("auctions.makeAnOffer")}</p>
          <div className="auction-section-offer">
            <input
              type="number"
              name="amount"
              id="aso-amount"
              min={auction.currentPrice}
              placeholder={`${auction.currentPrice}`}
              step={10}
              onChange={(e) =>
                setOfferRequest({
                  ...offerRequest,
                  amount: Number(e.target.value),
                })
              }
            />
            <button title={t("auctions.sendOfferTitle")} onClick={handleSendAnOffer}>
              <AiOutlineSend />
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default AuctionSection;
