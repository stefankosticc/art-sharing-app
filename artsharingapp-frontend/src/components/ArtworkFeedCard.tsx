import { useState } from "react";
import { FollowedUserArtworkResponse } from "../services/artwork";
import "../styles/ArtworkFeedCard.css";
import { ARTWORK_FALLBACK_IMAGE, BACKEND_BASE_URL } from "../config/constants";
import { useNavigate } from "react-router-dom";

type ArtworkFeedCardProps = {
  artwork: FollowedUserArtworkResponse;
};

const ArtworkFeedCard = ({ artwork }: ArtworkFeedCardProps) => {
  const [imgSrc, setImgSrc] = useState<string>(
    `${BACKEND_BASE_URL}${artwork.image}`
  );

  const navigate = useNavigate();

  return (
    <div
      className="afc-container"
      style={{ "--artwork-color": artwork.color } as React.CSSProperties}
      title={artwork.title}
      onClick={() => navigate(`/artwork/${artwork.id}`)}
    >
      <img
        src={imgSrc}
        alt={artwork.title || "artwork image"}
        onError={() => setImgSrc(ARTWORK_FALLBACK_IMAGE)}
        className="afc-img"
      />

      <div className="afc-description">
        <p>{artwork.title}</p>
        <p>@{artwork.postedByUserName}</p>
      </div>
    </div>
  );
};

export default ArtworkFeedCard;
