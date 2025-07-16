import { useState } from "react";
import "../styles/PutOnSaleModal.css";
import { Currency } from "../services/enums";
import { putArtworkOnSale, PutArtworkOnSaleRequest } from "../services/artwork";
import { AuctionStartDTO } from "../services/auction";

type PutOnSaleModalProps = {
  onClose: () => void;
  artworkId: number;
  refetchArtwork: () => void;
};

const PutOnSaleModal = ({
  onClose,
  artworkId,
  refetchArtwork,
}: PutOnSaleModalProps) => {
  const [activeTab, setActiveTab] = useState<string>("fixed");
  const [fixedPrice, setFixedPrice] = useState<number>(0);
  const [fixedCurrency, setFixedCurrency] = useState<Currency>(Currency.USD);

  const [auctionData, setAuctionData] = useState<AuctionStartDTO>({
    startTime: "",
    endTime: "",
    startingPrice: 0,
    currency: Currency.USD,
  });

  const handleSave = async () => {
    if (!artworkId) return;

    if (activeTab === "fixed") {
      const request: PutArtworkOnSaleRequest = {
        isOnSale: true,
        price: fixedPrice,
        currency: fixedCurrency,
      };
      const success = await putArtworkOnSale(artworkId, request);
      if (success) {
        onClose();
        refetchArtwork();
      }
    } else {
      // const success = await startAnAuction(artworkId, auctionData);
      // if (success) onClose();
    }
  };

  return (
    <div className="psm-page">
      <div className="psm-container">
        <div className="psm-tabs">
          <button
            className={activeTab === "fixed" ? "active" : ""}
            onClick={() => setActiveTab("fixed")}
          >
            Fixed Price
          </button>
          <button
            className={activeTab === "auction" ? "active" : ""}
            onClick={() => setActiveTab("auction")}
          >
            Auction
          </button>
        </div>

        {activeTab === "fixed" && (
          <div className="psm-fixed-form">
            <div className="psm-fixed-form-field">
              <label htmlFor="psm-fixed-price">Price:</label>
              <input
                type="number"
                id="psm-fixed-price"
                placeholder={"0"}
                onChange={(e) => setFixedPrice(Number(e.target.value))}
              />
            </div>
            <div className="psm-fixed-form-field">
              <label htmlFor="currency">Currency:</label>
              <select
                id="currency"
                value={fixedCurrency}
                className="psm-fixed-currency-select"
                onChange={(e) => setFixedCurrency(Number(e.target.value))}
              >
                {Object.keys(Currency)
                  .filter((key) => isNaN(Number(key)))
                  .map((cur) => (
                    <option
                      key={cur}
                      value={Currency[cur as keyof typeof Currency]}
                    >
                      {cur}
                    </option>
                  ))}
              </select>
            </div>
          </div>
        )}

        <div className="psm-buttons">
          <button id="psm-cancel" onClick={onClose}>
            Cancel
          </button>
          <button id="psm-save" onClick={handleSave}>
            Save
          </button>
        </div>
      </div>
    </div>
  );
};

export default PutOnSaleModal;
