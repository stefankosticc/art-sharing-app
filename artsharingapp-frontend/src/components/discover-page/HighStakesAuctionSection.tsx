import { useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { HighStakesAuctionResponse } from "../../services/discover";
import "./styles/HighStakesAuctionsSection.css";
import { Currency } from "../../services/enums";

type HighStakesAuctionsSectionProps = {
  auctions: HighStakesAuctionResponse[];
};

const HighStakesAuctionsSection = ({
  auctions,
}: HighStakesAuctionsSectionProps) => {
  const navigate = useNavigate();
  const { t } = useTranslation();

  return (
    <div className="hsa-container discover-container">
      {auctions.map((auction) => (
        <div
          className="hsa-auction"
          onClick={() => navigate(`/artwork/${auction.artworkId}`)}
          key={auction.auctionId}
        >
          <div className="hsa-title">{auction.artworkTitle}</div>
          <div className="hsa-info">
            <div className="hsa-info-column">
              <p>{t("common.currentPrice")}</p>
              <span>
                {auction.currentPrice.toLocaleString("en-US")}{" "}
                {Currency[auction.currency]}
              </span>
            </div>

            <div className="hsa-info-column">
              <p>{t("common.offerCount")}</p>
              <span>{auction.offerCount}</span>
            </div>
          </div>
        </div>
      ))}
    </div>
  );
};

export default HighStakesAuctionsSection;
