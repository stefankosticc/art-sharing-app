import { useState } from "react";
import { useTranslation } from "react-i18next";
import { ArtworkCardData, FavoriteArtwork } from "../services/artwork";
import "../styles/ArtworkCard.css";
import { useNavigate } from "react-router-dom";
import { ARTWORK_FALLBACK_IMAGE, IMAGE_SERVICE_BASE_URL } from "../config/constants";

type ArtworkCardProps = {
  artwork: ArtworkCardData | FavoriteArtwork | null;
  loading?: boolean;
};

const ArtworkCard = ({ artwork, loading = false }: ArtworkCardProps) => {
  const { t } = useTranslation();
  const navigate = useNavigate();

  const getTitle = (): string | null => {
    if (!artwork) return null;
    return "title" in artwork ? artwork.title : artwork.artworkTitle;
  };

  const getImage = (): string | null => {
    if (!artwork) return null;
    return "image" in artwork
      ? `${IMAGE_SERVICE_BASE_URL}${artwork.image}`
      : `${IMAGE_SERVICE_BASE_URL}${artwork.artworkImage}`;
  };

  const getId = (): number | null => {
    if (!artwork) return null;
    return "id" in artwork ? artwork.id : artwork.artworkId;
  };

  const [imgSrc, setImgSrc] = useState(getImage() || ARTWORK_FALLBACK_IMAGE);

  if (loading) {
    return (
      <div className="artwork-card">
        <div className="artwork-card-image skeleton" />
        <div className="skeleton-artwork-card-details skeleton" />
      </div>
    );
  }

  return (
    <div
      className="artwork-card"
      title={getTitle() || ""}
      onClick={() => navigate(`/artwork/${getId()}`)}
    >
      <img
        className="artwork-card-image"
        src={imgSrc}
        alt={getTitle() || t("common.artworkImageFallbackAlt")}
        onError={() => setImgSrc(ARTWORK_FALLBACK_IMAGE)}
      />
      <div className="artwork-card-details">
        <p>{getTitle() || t("common.artworkTitleNotFound")}</p>
      </div>
    </div>
  );
};

export default ArtworkCard;
