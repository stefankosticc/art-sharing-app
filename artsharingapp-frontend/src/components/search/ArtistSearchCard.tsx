import { UserSearchResponse } from "../../services/user";
import "./styles/ArtistSearchCard.css";

const fallbackImage =
  "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png";

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
          src={fallbackImage}
          alt="Default profile picture"
          className="artist-sc-picture"
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
