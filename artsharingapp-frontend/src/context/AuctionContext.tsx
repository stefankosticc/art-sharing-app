import { createContext, useContext, useState, ReactNode } from "react";
import { AuctionResponse } from "../services/auction";
import { useActiveAuction } from "../hooks/useActiveAuction";

type AuctionContextType = {
  auction: AuctionResponse | null;
  loadingAuction: boolean;
  refetchAuction: number;
  triggerRefetchAuction: () => void;
  artworkTitle: string;
};

const AuctionContext = createContext<AuctionContextType | undefined>(undefined);

export const AuctionProvider = ({
  artworkId,
  artworkTitle,
  children,
}: {
  artworkId: number;
  artworkTitle: string;
  children: ReactNode;
}) => {
  const [refetchAuction, setRefetchAuction] = useState(0);

  const triggerRefetchAuction = () => setRefetchAuction((prev) => prev + 1);

  const { auction, loadingAuction } = useActiveAuction(
    artworkId,
    refetchAuction,
  );

  return (
    <AuctionContext.Provider
      value={{
        auction,
        loadingAuction,
        refetchAuction,
        triggerRefetchAuction,
        artworkTitle,
      }}
    >
      {children}
    </AuctionContext.Provider>
  );
};

export const useAuctionContext = () => {
  const context = useContext(AuctionContext);
  if (!context) {
    throw new Error("useAuctionContext must be used within AuctionProvider");
  }
  return context;
};
