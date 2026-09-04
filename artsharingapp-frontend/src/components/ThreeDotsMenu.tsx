import { useRef, useState } from "react";
import { useTranslation } from "react-i18next";
import { useClickOutside } from "../hooks/useClickOutside";
import "../styles/ThreeDotsMenu.css";
import { deleteArtwork, removeArtworkFromSale } from "../services/artwork";
import { useNavigate } from "react-router-dom";
import PutOnSaleModal from "./auctions-and-sales/PutOnSaleModal";
import AuctionAnalyticsModal from "./auctions-and-sales/AuctionAnalyticsModal";
import TransferModal from "./auctions-and-sales/TransferModal";
import { toast } from "react-toastify";

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
  const { t } = useTranslation();
  const [isSaleModalOpen, setIsSaleModalOpen] = useState<boolean>(false);
  const [isAnalyticsModalOpen, setIsAnalyticsModalOpen] =
    useState<boolean>(false);
  const [isTransferModalOpen, setIsTransferModalOpen] =
    useState<boolean>(false);

  const threeDotMenuRef = useRef<HTMLDivElement>(null);
  useClickOutside(threeDotMenuRef, () => {
    if (!isSaleModalOpen && !isAnalyticsModalOpen && !isTransferModalOpen) {
      onClose();
    }
  });
  const navigate = useNavigate();

  const handleRemoveFromSale = async () => {
    const success = await removeArtworkFromSale(artworkId);
    if (success) {
      onClose();
      refetchArtwork();
      toast.success(t("artwork.removedFromSaleSuccess"));
    }
  };

  const handleDelete = async () => {
    const success = await deleteArtwork(artworkId);
    if (success) {
      onClose();
      navigate(-1);
      toast.success(t("artwork.deletedSuccess"));
    }
  };

  const menuOptions: {
    key: string;
    labelKey: string;
    action: () => any;
  }[] = [
    {
      key: "putOnSale",
      labelKey: "artwork.putOnSale",
      action: () => setIsSaleModalOpen(true),
    },
    {
      key: "removeFromSale",
      labelKey: "artwork.removeFromSale",
      action: handleRemoveFromSale,
    },
    {
      key: "auctionAnalytics",
      labelKey: "artwork.auctionAnalytics",
      action: () => setIsAnalyticsModalOpen(true),
    },
    {
      key: "transfer",
      labelKey: "artwork.transfer",
      action: () => setIsTransferModalOpen(true),
    },
    { key: "delete", labelKey: "common.delete", action: handleDelete },
  ];

  return (
    <>
      <div className="threeDots-menu" ref={threeDotMenuRef}>
        {menuOptions.map((option) => (
          <p
            className={`threeDots-menu-option ${
              option.key === "delete" ? "threeDots-menu-option-delete" : ""
            }`}
            key={option.key}
            onClick={option.action}
          >
            {t(option.labelKey)}
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
          onClose={() => {
            setIsAnalyticsModalOpen(false);
            onClose();
          }}
        />
      )}

      {isTransferModalOpen && (
        <TransferModal
          artworkId={artworkId}
          onClose={() => {
            setIsTransferModalOpen(false);
            onClose();
          }}
          refetchArtwork={refetchArtwork}
        />
      )}
    </>
  );
};

export default ThreeDotsMenu;
