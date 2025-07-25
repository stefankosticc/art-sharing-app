import { ARTIST_FALLBACK_IMAGE } from "../../config/constants";
import { TopArtistResponse } from "../../services/discover";
import "./styles/TopArtistsSection.css";

type TopArtistsSectionProps = {
  artists: TopArtistResponse[];
};

const TopArtistsSection = ({ artists }: TopArtistsSectionProps) => {
  return (
    <div className="top-artists-container">
      {artists.map((artist) => (
        <div className="top-artist" key={artist.id}>
          <img src={ARTIST_FALLBACK_IMAGE} alt={artist.name} />
          <p>{artist.name}</p>
        </div>
      ))}
    </div>
  );
};

export default TopArtistsSection;
