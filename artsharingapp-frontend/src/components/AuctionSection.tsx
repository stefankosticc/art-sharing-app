import "../styles/AuctionSection.css";
import { AiOutlineSend } from "react-icons/ai";

const AuctionSection = () => {
  return (
    <div className="auction-section-container">
      <div className="auction-section-header">
        <h4>Active Auction </h4>
        <p className="auction-section-time" title="2025-04-12 → 2025-05-12">
          Time left: 2 days
        </p>
      </div>
      <hr />
      <div className="auction-section-content">
        <div className="auction-section-column">
          <p>Starting Price</p>
          <p className="auction-section-column-info">300</p>
        </div>
        <div className="auction-section-column">
          <p>No. of Offers</p>
          <p className="auction-section-column-info">1</p>
        </div>
        <div className="auction-section-column">
          <p>Make an Offer</p>
          <div className="auction-section-offer">
            <input
              type="number"
              name="amount"
              id="aso-amount"
              min={"300"}
              step={"10"}
              placeholder="300"
            />
            <button title="Send offer">
              <AiOutlineSend />
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default AuctionSection;
