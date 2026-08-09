import { useRef } from "react";
import { toast } from "react-toastify";
import { PiDotsThreeOutlineVerticalFill } from "react-icons/pi";
import Countdown from "../Countdown";
import { useClickOutside } from "../../hooks/useClickOutside";
import { Currency, OfferStatus } from "../../services/enums";
import "./styles/MadeOfferFeedCard.css";

export type MockOffer = {
  id: number;
  auctionId: number;
  artworkId: number;
  artworkTitle: string;
  artworkImage: string;
  artistUserName: string;
  amount: number;
  currency: Currency;
  status: OfferStatus;
  auctionEndTime: Date;
};

type MadeOfferFeedCardProps = {
  offer: MockOffer;
  isMenuOpen: boolean;
  onOpenChange: (open: boolean) => void;
};

const MadeOfferFeedCard = ({
  offer,
  isMenuOpen,
  onOpenChange,
}: MadeOfferFeedCardProps) => {
  const menuRef = useRef<HTMLDivElement>(null);
  useClickOutside(menuRef, () => onOpenChange(false));

  return (
    <div className="moc-container">
      <img
        src={offer.artworkImage}
        alt={offer.artworkTitle}
        className="moc-img"
      />

      <div className="moc-body">
        <p className="moc-title">{offer.artworkTitle}</p>
        <p className="moc-artist">@{offer.artistUserName}</p>
      </div>

      <div className="moc-highlight">
        <p className="moc-time">
          Time left: <Countdown endTime={offer.auctionEndTime} />
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
          {OfferStatus[offer.status]}
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
                toast.info("Opening the artwork isn't wired up yet.");
              }}
            >
              Open Artwork
            </p>
            {offer.status === OfferStatus.SUBMITTED && (
              <p
                className="moc-menu-option moc-menu-option-danger"
                onClick={() => {
                  onOpenChange(false);
                  toast.info("Withdrawing offers isn't available yet.");
                }}
              >
                Withdraw
              </p>
            )}
          </div>
        )}
      </div>
    </div>
  );
};

export default MadeOfferFeedCard;
