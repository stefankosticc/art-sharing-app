import { Artwork } from "../../services/artwork";
import { Currency } from "../../services/enums";
import "../../styles/AuctionSection.css";
import "../../styles/FixedSaleSection.css";
import { AiOutlineSend } from "react-icons/ai";

type FixedSaleSectionProps = {
  artwork: Artwork;
};

const FixedSaleSection = ({ artwork }: FixedSaleSectionProps) => {
  return (
    <div className="auction-section-container fixed-sale-section-container">
      <h4>On Sale</h4>
      <hr />
      <div className="auction-section-content">
        <div className="auction-section-column">
          <p>Price</p>
          <p className="auction-section-column-info">
            {artwork.price?.toLocaleString("en-US")}{" "}
            {Currency[artwork.currency]}
          </p>
        </div>
        <div className="auction-section-column">
          <p>Send a message</p>
          <div className="auction-section-offer fixed-sale-section-message">
            <button title="Send a message">
              <AiOutlineSend />
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default FixedSaleSection;
