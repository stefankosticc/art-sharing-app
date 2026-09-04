import { useState } from "react";
import { useTranslation } from "react-i18next";
import { FiSearch } from "react-icons/fi";
import "./styles/TransferModal.css";
import { useSearch } from "../../hooks/useSearch";
import ArtistSearchCard from "../search/ArtistSearchCard";
import { UserSearchResponse } from "../../services/user";
import { transferArtwork } from "../../services/artwork";
import { toast } from "react-toastify";

type TransferModalProps = {
  artworkId: number;
  onClose: () => void;
  refetchArtwork: () => void;
};

const TransferModal = ({
  artworkId,
  onClose,
  refetchArtwork,
}: TransferModalProps) => {
  const { t } = useTranslation();
  const [searchString, setSearchString] = useState<string>("");

  const { results } = useSearch({
    searchString: searchString,
    filter: "artist",
  });

  const handleTransfer = async (
    artworkId: number,
    user: UserSearchResponse
  ) => {
    if (
      window.confirm(
        t("auctions.transferConfirm", { userName: user.userName })
      )
    ) {
      const success = await transferArtwork(artworkId, user.id);
      if (success) {
        onClose();
        refetchArtwork();
        toast.success(
          t("auctions.transferSuccess", { userName: user.userName })
        );
      }
    }
  };

  return (
    <div className="blured-page">
      <div className="modal-container transfer-modal-container">
        <div className="search-bar">
          <input
            type="text"
            placeholder={t("auctions.transferSearchPlaceholder")}
            value={searchString}
            onChange={(e) => setSearchString(e.target.value)}
            autoFocus
          />
          <button type="submit">
            <FiSearch />
          </button>
        </div>

        <div className="search-results">
          {results &&
            results.map((result) => (
              <ArtistSearchCard
                key={result.id}
                artist={result as UserSearchResponse}
                onClick={() =>
                  handleTransfer(artworkId, result as UserSearchResponse)
                }
              />
            ))}
        </div>

        <button className="transfer-cancel-btn" onClick={onClose}>
          {t("common.cancel")}
        </button>
      </div>
    </div>
  );
};

export default TransferModal;
