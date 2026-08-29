import { useTranslation } from "react-i18next";
import { AuctionResponse, AuctionUpdateRequest } from "../../services/auction";
import { Currency } from "../../services/enums";
import "./styles/EditAuctionForm.css";

type NewAuctionFormProps = {
  auction: AuctionResponse;
  auctionData: AuctionUpdateRequest;
  setAuctionData: React.Dispatch<React.SetStateAction<AuctionUpdateRequest>>;
  handleEndAuction: () => void;
};

const EditAuctionForm = ({
  auction,
  auctionData,
  setAuctionData,
  handleEndAuction,
}: NewAuctionFormProps) => {
  const { t } = useTranslation();

  return (
    <div className="psm-auction-form">
      <h4 className="psm-auction-title">{t("auctions.editCurrentAuction")}</h4>

      <div className="psm-form-field">
        <p className="psm-edit-auction-label">
          {t("auctions.startingPriceLabel")}
        </p>
        <div className="psm-edit-auction-data">
          {auction.currentPrice.toLocaleString("en-US")}
        </div>
      </div>

      <div className="psm-form-field">
        <p className="psm-edit-auction-label">{t("auctions.currencyLabel")}</p>
        <div className="psm-currency-select psm-edit-auction-currency">
          {Currency[auction.currency]}
        </div>
      </div>

      <div className="psm-auction-time">
        <div className="psm-form-field">
          <p className="psm-edit-auction-label">{t("auctions.startTimeLabel")}</p>
          <div className="psm-edit-auction-data">
            {new Date(auction.startTime).toLocaleString("en-GB", {
              year: "numeric",
              month: "numeric",
              day: "numeric",
              hour: "2-digit",
              minute: "2-digit",
            })}
          </div>
        </div>
        <span>→</span>
        <div className="psm-form-field">
          <label htmlFor="auction-end">{t("auctions.endTimeLabel")}</label>
          <input
            id="auction-end"
            type="datetime-local"
            min={new Date().toISOString().slice(0, 16)}
            value={
              auctionData.endTime
                ? new Date(auctionData.endTime)
                    .toLocaleString("sv-SE")
                    .replace(" ", "T")
                    .slice(0, 16)
                : ""
            }
            onChange={(e) =>
              setAuctionData({
                endTime: e.target.value
                  ? new Date(e.target.value)
                  : new Date(auction.endTime),
              })
            }
          />
        </div>
      </div>
      <button id="psm-end-auction-btn" onClick={handleEndAuction}>
        {t("auctions.endAuctionButton")}
      </button>
    </div>
  );
};

export default EditAuctionForm;
