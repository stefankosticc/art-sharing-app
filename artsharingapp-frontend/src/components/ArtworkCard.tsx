import { useState } from "react";
import { ArtworkCardData } from "../services/artwork";
import "../styles/ArtworkCard.css";

type ArtworkCardProps = {
  artwork: ArtworkCardData | null;
  loading?: boolean;
};

const ArtworkCard = ({ artwork, loading = false }: ArtworkCardProps) => {
  const fallbackImage =
    "https://upload.wikimedia.org/wikipedia/commons/a/a3/Image-not-found.png?20210521171500";

  const [imgSrc, setImgSrc] = useState(artwork?.image || fallbackImage);

  if (loading) {
    return (
      <div className="artwork-card">
        <div className="artwork-card-image skeleton" />
        <div className="skeleton-artwork-card-details skeleton" />
      </div>
    );
  }

  return (
    <div className="artwork-card" title={artwork?.title || ""}>
      <img
        className="artwork-card-image"
        src={imgSrc}
        alt={artwork?.title || "artwork image not found"}
        onError={() => setImgSrc(fallbackImage)}
      />
      <div className="artwork-card-details">
        <p>{artwork?.title || "Error: Title Not Found"}</p>
      </div>
    </div>
  );
};

export default ArtworkCard;
