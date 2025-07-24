import { useState, useEffect } from "react";
import {
  FollowedUserArtworkResponse,
  getFollowedUsersArtworks,
} from "../services/artwork";

export const useFollowedUsersArtworks = (refetch: boolean = false) => {
  const [artworks, setArtworks] = useState<FollowedUserArtworkResponse[]>([]);
  const [loadingArtworks, setLoadingArtworks] = useState<boolean>(false);
  const [skip, setSkip] = useState(0);
  const take = 30;
  const [hasMore, setHasMore] = useState(true);

  useEffect(() => {
    let isCancelled = false;

    const fetchArtworks = async () => {
      try {
        setLoadingArtworks(true);
        const initialData = await getFollowedUsersArtworks(0, take);
        if (!isCancelled) {
          setArtworks(initialData);
          setSkip(initialData.length);
          setHasMore(initialData.length === take);
        }
      } catch (err) {
        if (!isCancelled) {
          console.error("Failed to fetch artworks.");
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
  }, [refetch]);

  const loadMoreArtworks = async () => {
    if (loadingArtworks || !hasMore) return;

    setLoadingArtworks(true);
    try {
      const newArtworks = await getFollowedUsersArtworks(skip, take);
      setArtworks((prev) => [...prev, ...newArtworks]);
      setSkip((prev) => prev + newArtworks.length);
      if (newArtworks.length < take) setHasMore(false);
    } catch (err) {
      console.error("Failed to load more artworks.");
    } finally {
      setLoadingArtworks(false);
    }
  };

  return {
    artworks,
    loadingArtworks,
    loadMoreArtworks,
  };
};
