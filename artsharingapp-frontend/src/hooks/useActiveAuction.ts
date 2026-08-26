import { useState, useEffect } from "react";
import {
  AuctionResponse,
  auctionHubService,
  getActiveAuction,
} from "../services/auction";

export const useActiveAuction = (artworkId: number, refetchAuction: number) => {
  const [auction, setAuction] = useState<AuctionResponse | null>(null);
  const [loadingAuction, setLoadingAuction] = useState<boolean>(false);

  useEffect(() => {
    let isCancelled = false;

    const fetchAuction = async () => {
      if (artworkId <= 0) {
        setAuction(null);
        setLoadingAuction(false);
        return;
      }
      try {
        setLoadingAuction(true);
        const response = await getActiveAuction(artworkId);
        if (!isCancelled) {
          setAuction(response);
        }
      } catch (err) {
        if (!isCancelled) {
          console.error("Failed to fetch auction.");
          setAuction(null);
        }
      } finally {
        if (!isCancelled) {
          setLoadingAuction(false);
        }
      }
    };

    fetchAuction();

    return () => {
      isCancelled = true;
    };
  }, [artworkId, refetchAuction]);

  useEffect(() => {
    if (!auction) return;

    const accessToken = localStorage.getItem("accessToken");
    if (!accessToken) return;

    const auctionId = auction.id;
    let isCancelled = false;

    auctionHubService.start(accessToken).then(() => {
      if (!isCancelled) auctionHubService.joinAuction(auctionId);
    });

    const unsubscribe = auctionHubService.subscribe((update) => {
      if (update.id === auctionId) {
        setAuction(update);
      }
    });

    return () => {
      isCancelled = true;
      auctionHubService.leaveAuction(auctionId);
      unsubscribe();
    };
  }, [auction?.id]);

  return { auction, loadingAuction };
};
