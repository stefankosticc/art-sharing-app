import { useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";
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
  const { t } = useTranslation();
  const navigate = useNavigate();

  const handleAccept = async () => {
    if (window.confirm(t("auctions.acceptOfferConfirm"))) {
      const success = await acceptOffer(offer.id);
      if (success) {
        onClose();
        navigate("/chat", {
          state: {
            selectedUser: {
              userId: offer.userId,
              userName: offer.userName,
              profilePhoto: offer.userProfilePhoto,
            },
            input: t("auctions.acceptOfferMessage", {
              amount: offer.amount.toLocaleString("en-US"),
              currency: currency !== undefined ? Currency[currency] : "",
            }),
          },
        });
      }
    }
  };

  const handleReject = async () => {
    const success = await rejectOffer(offer.id);
    if (success) refetchOffers();
  };

  return (
    <div className="oc-container">
      <p className="oc-top-offer">🔥 {t("auctions.topOffer")}</p>
      <span className="oc-status">
        {t(`auctions.offerStatus.${OfferStatus[offer.status].toLowerCase()}`)}
      </span>
      <p>@{offer.userName}</p>
      <p>
        {offer.amount.toLocaleString("en-US")}{" "}
        {currency !== undefined && Currency[currency]}
      </p>

      <div className="oc-actions">
        <button
          id="accept-offer"
          title={t("auctions.accept")}
          onClick={handleAccept}
          disabled={offer.status != OfferStatus.SUBMITTED ? true : false}
        >
          {t("auctions.accept")}
        </button>
        <button
          id="reject-offer"
          title={t("auctions.reject")}
          onClick={handleReject}
          disabled={offer.status != OfferStatus.SUBMITTED ? true : false}
        >
          {t("auctions.reject")}
        </button>
      </div>
    </div>
  );
};

export default OfferCard;
