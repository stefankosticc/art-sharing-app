import { useState, useEffect } from "react";
import { ActiveAuctionResponse, getActiveAuctions } from "../services/auction";

export const useActiveAuctions = () => {
  const [auctions, setAuctions] = useState<ActiveAuctionResponse[]>([]);
  const [loadingAuctions, setLoadingAuctions] = useState<boolean>(false);
  const [skip, setSkip] = useState(0);
  const take = 20;
  const [hasMore, setHasMore] = useState(true);

  useEffect(() => {
    let isCancelled = false;

    const fetchAuctions = async () => {
      try {
        setLoadingAuctions(true);
        const initialData = await getActiveAuctions(0, take);
        if (!isCancelled) {
          setAuctions(initialData);
          setSkip(initialData.length);
          setHasMore(initialData.length === take);
        }
      } catch (err) {
        if (!isCancelled) {
          console.error("Failed to fetch active auctions.");
          setAuctions([]);
        }
      } finally {
        if (!isCancelled) {
          setLoadingAuctions(false);
        }
      }
    };

    fetchAuctions();

    return () => {
      isCancelled = true;
    };
  }, []);

  const loadMoreAuctions = async () => {
    if (loadingAuctions || !hasMore) return;

    setLoadingAuctions(true);
    try {
      const newAuctions = await getActiveAuctions(skip, take);
      setAuctions((prev) => [...prev, ...newAuctions]);
      setSkip((prev) => prev + newAuctions.length);
      if (newAuctions.length < take) setHasMore(false);
    } catch (err) {
      console.error("Failed to load more active auctions.");
    } finally {
      setLoadingAuctions(false);
    }
  };

  return { auctions, loadingAuctions, loadMoreAuctions };
};
