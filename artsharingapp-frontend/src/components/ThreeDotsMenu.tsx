import { useRef, useState } from "react";
import { useClickOutside } from "../hooks/useClickOutside";
import "../styles/ThreeDotsMenu.css";
import { deleteArtwork, removeArtworkFromSale } from "../services/artwork";
import { useNavigate } from "react-router-dom";
import PutOnSaleModal from "./auctions-and-sales/PutOnSaleModal";
import AuctionAnalyticsModal from "./auctions-and-sales/AuctionAnalyticsModal";

type ThreeDotsMenuProps = {
  onClose: () => void;
  artworkId: number;
  refetchArtwork: () => void;
};

const ThreeDotsMenu = ({
  onClose,
  artworkId,
  refetchArtwork,
}: ThreeDotsMenuProps) => {
  const [isSaleModalOpen, setIsSaleModalOpen] = useState<boolean>(false);
  const [isAnalyticsModalOpen, setIsAnalyticsModalOpen] =
    useState<boolean>(false);

  const threeDotMenuRef = useRef<HTMLDivElement>(null);
  useClickOutside(threeDotMenuRef, () => {
    if (!isSaleModalOpen && !isAnalyticsModalOpen) {
      onClose();
    }
  });
  const navigate = useNavigate();

  const handleRemoveFromSale = async () => {
    const success = await removeArtworkFromSale(artworkId);
    if (success) {
      onClose();
      refetchArtwork();
    }
  };

  const handleDelete = async () => {
    const success = await deleteArtwork(artworkId);
    if (success) {
      onClose();
      navigate(-1);
    }
  };

  const menuOptions: {
    label: string;
    action: () => any;
  }[] = [
    { label: "Put On Sale", action: () => setIsSaleModalOpen(true) },
    { label: "Remove From Sale", action: handleRemoveFromSale },
    { label: "Auction Analytics", action: () => setIsAnalyticsModalOpen(true) },
    { label: "Transfer", action: () => {} },
    { label: "Delete", action: handleDelete },
  ];

  return (
    <>
      <div className="threeDots-menu" ref={threeDotMenuRef}>
        {menuOptions.map((option) => (
          <p
            className={`threeDots-menu-option ${
              option.label === "Delete" ? "threeDots-menu-option-delete" : ""
            }`}
            key={option.label}
            onClick={option.action}
          >
            {option.label}
          </p>
        ))}
      </div>

      {isSaleModalOpen && (
        <PutOnSaleModal
          onClose={() => {
            setIsSaleModalOpen(false);
            onClose();
          }}
          artworkId={artworkId}
          refetchArtwork={refetchArtwork}
        />
      )}

      {isAnalyticsModalOpen && (
        <AuctionAnalyticsModal
          artworkId={artworkId}
          onClose={() => {
            setIsAnalyticsModalOpen(false);
            onClose();
          }}
        />
      )}
    </>
  );
};

export default ThreeDotsMenu;
