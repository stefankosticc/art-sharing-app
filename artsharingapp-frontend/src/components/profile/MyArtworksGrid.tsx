import { useEffect, useState } from "react";
import { ArtworkCardData, getMyArtworks } from "../../services/artwork";
import ArtworkCard from "../ArtworkCard";
import { User } from "../../services/auth";

interface MyArtworksGridProps {
  activeTab: string;
  user: User | null;
  isMyProfile: boolean;
}

const MyArtworksGrid = ({
  activeTab,
  user,
  isMyProfile,
}: MyArtworksGridProps) => {
  const [artworks, setArtworks] = useState<ArtworkCardData[] | null>(null);
  const [loading, setLoading] = useState<boolean>(true);

  useEffect(() => {
    const fetchArtworks = async () => {
      try {
        const response = await getMyArtworks();
        setArtworks(response);
      } catch (err) {
        setLoading(true);
      } finally {
        setLoading(false);
      }
    };

    fetchArtworks();
  }, []);

  return (
    <div
      className={`profile-content${activeTab === "artworks" ? " active" : ""}`}
    >
      {artworks && artworks.length !== 0 ? (
        <div className="profile-artwork-grid">
          {artworks.map((artwork) => (
            <ArtworkCard key={artwork.id} artwork={artwork} loading={loading} />
          ))}
        </div>
      ) : (
        <p className="profile-content-text not-found">
          You have no artworks yet.
        </p>
      )}
    </div>
  );
};

export default MyArtworksGrid;
