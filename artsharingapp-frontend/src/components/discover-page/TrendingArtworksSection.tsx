import { useNavigate } from "react-router-dom";
import "./styles/TrendingArtworksSection.css";
import { Currency } from "../../services/enums";
import { DiscoverArtworkResponse } from "../../services/artwork";
import ArtworkFeedCard from "./ArtworkFeedCard";

type TrendingArtworksSectionProps = {
  artworks: DiscoverArtworkResponse[];
};

const TrendingArtworksSection = ({
  artworks,
}: TrendingArtworksSectionProps) => {
  return (
    <div className="tas-container discover-container">
      {artworks.map((artwork) => (
        <ArtworkFeedCard artwork={artwork} key={artwork.id} />
      ))}
    </div>
  );
};

export default TrendingArtworksSection;
