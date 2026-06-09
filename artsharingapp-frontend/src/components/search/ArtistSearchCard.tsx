import {
  ARTIST_FALLBACK_IMAGE,
  IMAGE_SERVICE_BASE_URL,
} from "../../config/constants";
import { UserSearchResponse } from "../../services/user";
import "./styles/ArtistSearchCard.css";

type ArtistSearchCardProps = {
  artist: UserSearchResponse;
  onClick?: () => void;
};

const ArtistSearchCard = ({ artist, onClick }: ArtistSearchCardProps) => {
  if (!artist) return null;
  return (
    <div className="artist-sc-container" onClick={onClick}>
      <div className="artist-sc-img-container">
        <img
          src={
            artist.profilePhoto
              ? `${IMAGE_SERVICE_BASE_URL}${artist.profilePhoto}`
              : ARTIST_FALLBACK_IMAGE
          }
          alt="Default profile picture"
          className="artist-sc-picture"
          onError={(e) => {
            e.currentTarget.src = ARTIST_FALLBACK_IMAGE;
          }}
        />
      </div>
      <div className="artist-sc-details">
        <p>{artist.name}</p>
        <span className="artist-sc-username">@{artist.userName}</span>
      </div>
    </div>
  );
};

export default ArtistSearchCard;
