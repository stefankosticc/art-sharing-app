import { useState, useEffect } from "react";
import { MyOfferResponse, getMyOffers, withdrawOffer } from "../services/auction";
import { OfferStatus } from "../services/enums";

export const useMyOffers = () => {
  const [offers, setOffers] = useState<MyOfferResponse[]>([]);
  const [loadingOffers, setLoadingOffers] = useState<boolean>(false);
  const [skip, setSkip] = useState(0);
  const take = 20;
  const [hasMore, setHasMore] = useState(true);

  useEffect(() => {
    let isCancelled = false;

    const fetchOffers = async () => {
      try {
        setLoadingOffers(true);
        const initialData = await getMyOffers(0, take);
        if (!isCancelled) {
          setOffers(initialData);
          setSkip(initialData.length);
          setHasMore(initialData.length === take);
        }
      } catch (err) {
        if (!isCancelled) {
          console.error("Failed to fetch made offers.");
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
  }, []);

  const loadMoreOffers = async () => {
    if (loadingOffers || !hasMore) return;

    setLoadingOffers(true);
    try {
      const newOffers = await getMyOffers(skip, take);
      setOffers((prev) => [...prev, ...newOffers]);
      setSkip((prev) => prev + newOffers.length);
      if (newOffers.length < take) setHasMore(false);
    } catch (err) {
      console.error("Failed to load more made offers.");
    } finally {
      setLoadingOffers(false);
    }
  };

  const withdraw = async (offerId: number) => {
    const success = await withdrawOffer(offerId);
    if (success) {
      setOffers((prev) =>
        prev.map((offer) =>
          offer.id === offerId
            ? { ...offer, status: OfferStatus.WITHDRAWN }
            : offer,
        ),
      );
    }
    return success;
  };

  return { offers, loadingOffers, loadMoreOffers, withdraw };
};
