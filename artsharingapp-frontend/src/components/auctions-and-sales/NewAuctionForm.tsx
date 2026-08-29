import { useTranslation } from "react-i18next";
import { AuctionStartRequest } from "../../services/auction";
import { Currency } from "../../services/enums";

type NewAuctionFormProps = {
  auctionData: AuctionStartRequest;
  setAuctionData: React.Dispatch<React.SetStateAction<AuctionStartRequest>>;
};

const NewAuctionForm = ({
  auctionData,
  setAuctionData,
}: NewAuctionFormProps) => {
  const { t } = useTranslation();

  return (
    <div className="psm-auction-form">
      <h4 className="psm-auction-title">{t("auctions.scheduleNewAuction")}</h4>
      <div className="psm-form-field">
        <label htmlFor="auction-starting-price">
          {t("auctions.startingPriceLabel")}
        </label>
        <input
          type="number"
          id="auction-starting-price"
          placeholder="0"
          onChange={(e) =>
            setAuctionData({
              ...auctionData,
              startingPrice: Number(e.target.value),
            })
          }
        />
      </div>
      <div className="psm-form-field">
        <label htmlFor="auction-currency">{t("auctions.currencyLabel")}</label>
        <select
          value={auctionData.currency}
          id="auction-currency"
          className="psm-currency-select"
          onChange={(e) =>
            setAuctionData({
              ...auctionData,
              currency: Number(e.target.value),
            })
          }
        >
          {Object.keys(Currency)
            .filter((key) => isNaN(Number(key)))
            .map((cur) => (
              <option key={cur} value={Currency[cur as keyof typeof Currency]}>
                {cur}
              </option>
            ))}
        </select>
      </div>

      <div className="psm-auction-time">
        <div className="psm-form-field">
          <label htmlFor="auction-start">{t("auctions.startTimeLabel")}</label>
          <input
            id="auction-start"
            type="datetime-local"
            min={new Date().toISOString().slice(0, 16)}
            onChange={(e) =>
              setAuctionData({
                ...auctionData,
                startTime: new Date(e.target.value),
              })
            }
          />
        </div>
        <span>→</span>
        <div className="psm-form-field">
          <label htmlFor="auction-end">{t("auctions.endTimeLabel")}</label>
          <input
            id="auction-end"
            type="datetime-local"
            onChange={(e) =>
              setAuctionData({
                ...auctionData,
                endTime: new Date(e.target.value),
              })
            }
          />
        </div>
      </div>
    </div>
  );
};

export default NewAuctionForm;
