import { useState, useEffect } from "react";
import { auctionHubService, getOffers, OfferResponse } from "../services/auction";

export const useOffers = (auctionId: number, refetch: boolean = false) => {
  const [offers, setOffers] = useState<OfferResponse[] | null>(null);
  const [loadingOffers, setLoadingOffers] = useState<boolean>(false);

  useEffect(() => {
    let isCancelled = false;

    const fetchOffers = async () => {
      if (auctionId <= 0) {
        setOffers(null);
        setLoadingOffers(false);
        return;
      }
      try {
        setLoadingOffers(true);
        const response = await getOffers(auctionId);
        if (!isCancelled) {
          setOffers(response);
        }
      } catch (err) {
        if (!isCancelled) {
          console.error("Failed to fetch offers.");
          setOffers([]);
        }
      } finally {
        if (!isCancelled) {
          setLoadingOffers(false);
        }
      }
    };

    fetchOffers();

    return () => {
      isCancelled = true;
    };
  }, [auctionId, refetch]);

  useEffect(() => {
    if (auctionId <= 0) return;

    const unsubscribe = auctionHubService.subscribeOfferUpdate((update) => {
      if (update.auctionId !== auctionId) return;

      setOffers((prev) => {
        const existing = prev ?? [];
        const index = existing.findIndex((offer) => offer.id === update.id);
        if (index === -1) return [update, ...existing];

        const next = [...existing];
        next[index] = update;
        return next;
      });
    });

    return unsubscribe;
  }, [auctionId]);

  return { offers, loadingOffers };
};
