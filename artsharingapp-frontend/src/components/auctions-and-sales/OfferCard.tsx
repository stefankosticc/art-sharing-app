import {
  acceptOffer,
  OfferResponse,
  rejectOffer,
} from "../../services/auction";
import { Currency, OfferStatus } from "../../services/enums";
import "./styles/OfferCard.css";

type OfferCardProps = {
  offer: OfferResponse;
  currency?: Currency;
  onClose: () => void;
  refetchOffers: () => void;
};

const OfferCard = ({
  offer,
  currency,
  onClose,
  refetchOffers,
}: OfferCardProps) => {
  const handleAccept = async () => {
    const success = await acceptOffer(offer.id);
    if (success) onClose();
  };

  const handleReject = async () => {
    const success = await rejectOffer(offer.id);
    if (success) refetchOffers();
  };

  return (
    <div className="oc-container">
      <p className="oc-top-offer">🔥 Top Offer</p>
      <span className="oc-status">{OfferStatus[offer.status]}</span>
      <p>@{offer.userName}</p>
      <p>
        {offer.amount.toLocaleString("en-US")} {currency && Currency[currency]}
      </p>

      <div className="oc-actions">
        <button
          id="accept-offer"
          title="Accept"
          onClick={handleAccept}
          disabled={offer.status != OfferStatus.SUBMITTED ? true : false}
        >
          Accept
        </button>
        <button
          id="reject-offer"
          title="Reject"
          onClick={handleReject}
          disabled={offer.status != OfferStatus.SUBMITTED ? true : false}
        >
          Reject
        </button>
      </div>
    </div>
  );
};

export default OfferCard;
