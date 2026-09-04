import { useRef, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { PiDotsThreeOutlineVerticalFill } from "react-icons/pi";
import {
  ARTWORK_FALLBACK_IMAGE,
  IMAGE_SERVICE_BASE_URL,
} from "../../config/constants";
import { MyOfferResponse } from "../../services/auction";
import Countdown from "../Countdown";
import { useClickOutside } from "../../hooks/useClickOutside";
import { Currency, OfferStatus } from "../../services/enums";
import "./styles/MadeOfferFeedCard.css";

type MadeOfferFeedCardProps = {
  offer: MyOfferResponse;
  isMenuOpen: boolean;
  onOpenChange: (open: boolean) => void;
  onWithdraw: (offerId: number) => void;
};

const MadeOfferFeedCard = ({
  offer,
  isMenuOpen,
  onOpenChange,
  onWithdraw,
}: MadeOfferFeedCardProps) => {
  const { t } = useTranslation();
  const navigate = useNavigate();
  const menuRef = useRef<HTMLDivElement>(null);
  useClickOutside(menuRef, () => onOpenChange(false));

  const [imgSrc, setImgSrc] = useState<string>(
    offer.artworkImage
      ? `${IMAGE_SERVICE_BASE_URL}${offer.artworkImage}`
      : ARTWORK_FALLBACK_IMAGE,
  );

  return (
    <div className="moc-container">
      <img
        src={imgSrc}
        alt={offer.artworkTitle}
        onError={() => setImgSrc(ARTWORK_FALLBACK_IMAGE)}
        className="moc-img"
      />

      <div className="moc-body">
        <p className="moc-title">{offer.artworkTitle}</p>
        <p className="moc-artist">@{offer.postedByUserName}</p>
      </div>

      <div className="moc-highlight">
        <p className="moc-time">
          {t("auctions.timeLeft")} <Countdown endTime={offer.auctionEndTime} />
        </p>
        <p className="moc-amount">
          {offer.amount.toLocaleString("en-US")}{" "}
          {Currency[offer.currency]}
        </p>
        <span
          className={`moc-status moc-status-${OfferStatus[
            offer.status
          ].toLowerCase()}`}
        >
          {t(
            `auctions.offerStatus.${OfferStatus[offer.status].toLowerCase()}`,
          )}
        </span>
      </div>

      <div className="moc-menu-container" ref={isMenuOpen ? menuRef : null}>
        <PiDotsThreeOutlineVerticalFill
          className="moc-menu-icon"
          onClick={(e) => {
            e.stopPropagation();
            onOpenChange(!isMenuOpen);
          }}
        />
        {isMenuOpen && (
          <div className="moc-menu">
            <p
              className="moc-menu-option"
              onClick={() => {
                onOpenChange(false);
                navigate(`/artwork/${offer.artworkId}`);
              }}
            >
              {t("auctions.openArtwork")}
            </p>
            {offer.status === OfferStatus.SUBMITTED && (
              <p
                className="moc-menu-option moc-menu-option-danger"
                onClick={() => {
                  onOpenChange(false);
                  if (window.confirm(t("auctions.withdrawConfirm"))) {
                    onWithdraw(offer.id);
                  }
                }}
              >
                {t("auctions.withdraw")}
              </p>
            )}
          </div>
        )}
      </div>
    </div>
  );
};

export default MadeOfferFeedCard;
