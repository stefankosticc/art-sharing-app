import { useState, useEffect } from "react";
import { OnSaleArtworkResponse, getOnSaleArtworks } from "../services/artwork";

export const useOnSaleArtworks = () => {
  const [artworks, setArtworks] = useState<OnSaleArtworkResponse[]>([]);
  const [loadingArtworks, setLoadingArtworks] = useState<boolean>(false);
  const [skip, setSkip] = useState(0);
  const take = 20;
  const [hasMore, setHasMore] = useState(true);

  useEffect(() => {
    let isCancelled = false;

    const fetchArtworks = async () => {
      try {
        setLoadingArtworks(true);
        const initialData = await getOnSaleArtworks(0, take);
        if (!isCancelled) {
          setArtworks(initialData);
          setSkip(initialData.length);
          setHasMore(initialData.length === take);
        }
      } catch (err) {
        if (!isCancelled) {
          console.error("Failed to fetch on-sale artworks.");
          setArtworks([]);
        }
      } finally {
        if (!isCancelled) {
          setLoadingArtworks(false);
        }
      }
    };

    fetchArtworks();

    return () => {
      isCancelled = true;
    };
  }, []);

  const loadMoreArtworks = async () => {
    if (loadingArtworks || !hasMore) return;

    setLoadingArtworks(true);
    try {
      const newArtworks = await getOnSaleArtworks(skip, take);
      setArtworks((prev) => [...prev, ...newArtworks]);
      setSkip((prev) => prev + newArtworks.length);
      if (newArtworks.length < take) setHasMore(false);
    } catch (err) {
      console.error("Failed to load more on-sale artworks.");
    } finally {
      setLoadingArtworks(false);
    }
  };

  return { artworks, loadingArtworks, loadMoreArtworks };
};
