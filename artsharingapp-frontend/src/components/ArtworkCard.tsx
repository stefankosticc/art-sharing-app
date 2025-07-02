import { useState } from "react";
import { ArtworkCardData, FavoriteArtwork } from "../services/artwork";
import "../styles/ArtworkCard.css";
import { useNavigate } from "react-router-dom";

type ArtworkCardProps = {
  artwork: ArtworkCardData | FavoriteArtwork | null;
  loading?: boolean;
};

const ArtworkCard = ({ artwork, loading = false }: ArtworkCardProps) => {
  const navigate = useNavigate();

  const fallbackImage =
    "https://upload.wikimedia.org/wikipedia/commons/a/a3/Image-not-found.png?20210521171500";

  const getTitle = (): string | null => {
    if (!artwork) return null;
    return "title" in artwork ? artwork.title : artwork.artworkTitle;
  };

  const getImage = (): string | null => {
    if (!artwork) return null;
    return "image" in artwork ? artwork.image : artwork.artworkImage;
  };

  const getId = (): number | null => {
    if (!artwork) return null;
    return "id" in artwork ? artwork.id : artwork.artworkId;
  };

  const [imgSrc, setImgSrc] = useState(getImage() || fallbackImage);

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
        alt={getTitle() || "artwork image not found"}
        onError={() => setImgSrc(fallbackImage)}
      />
      <div className="artwork-card-details">
        <p>{getTitle() || "Error: Title Not Found"}</p>
      </div>
    </div>
  );
};

export default ArtworkCard;
