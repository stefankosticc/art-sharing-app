import * as signalR from "@microsoft/signalr";
import { toast } from "react-toastify";
import { BACKEND_BASE_URL } from "../config/constants";
import authAxios from "./authAxios";
import { Currency, OfferStatus } from "./enums";

export interface AuctionStartRequest {
  startTime: Date;
  endTime: Date;
  startingPrice: number;
  currency: Currency;
}

export interface AuctionUpdateRequest {
  endTime: Date;
}

export interface AuctionResponse {
  id: number;
  startTime: Date;
  endTime: Date;
  currentPrice: number;
  offerCount: number;
  currency: Currency;
}

export interface ActiveAuctionResponse {
  auctionId: number;
  artworkId: number;
  artworkTitle: string;
  artworkImage: string;
  postedByUserName: string;
  currentPrice: number;
  offerCount: number;
  currency: Currency;
  endTime: Date;
}

export interface OfferRequest {
  amount: number;
}

export interface OfferResponse {
  id: number;
  auctionId: number;
  amount: number;
  userId: number;
  userName: string;
  userProfilePhoto: string | null;
  status: OfferStatus;
}

export async function startAnAuction(
  artworkId: number,
  auctionData: AuctionStartRequest,
): Promise<boolean> {
  try {
    await authAxios.post(`artwork/${artworkId}/auction/start`, auctionData);
    return true;
  } catch (error: any) {
    const message =
      error?.response?.data?.error ||
      error?.message ||
      "An unknown error occurred.";
    console.error("Error:", message);
    toast.error(message);
    return false;
  }
}

export async function getActiveAuction(
  artworkId: number,
): Promise<AuctionResponse> {
  const response = await authAxios.get(`artwork/${artworkId}/auction/active`);
  return response.data;
}

export async function getActiveAuctions(
  skip = 0,
  take = 20,
): Promise<ActiveAuctionResponse[]> {
  const response = await authAxios.get(`auctions/active`, {
    params: { skip, take },
  });
  return response.data;
}

export async function makeAnOffer(
  auctionId: number,
  request: OfferRequest,
): Promise<boolean> {
  try {
    await authAxios.post(`auction/${auctionId}/make-an-offer`, request);
    return true;
  } catch (error: any) {
    const message =
      error?.response?.data?.error ||
      error?.message ||
      "An unknown error occurred.";
    toast.error(message);
    console.error("Error:", message);
    return false;
  }
}

export async function updateAuction(
  auctionId: number,
  request: AuctionUpdateRequest,
): Promise<boolean> {
  try {
    await authAxios.put(`auction/${auctionId}`, request);
    return true;
  } catch (error: any) {
    const message =
      error?.response?.data?.error ||
      error?.message ||
      "An unknown error occurred.";
    console.error("Error:", message);
    toast.error(message);
    return false;
  }
}

export async function getOffers(auctionId: number): Promise<OfferResponse[]> {
  const response = await authAxios.get(`auction/${auctionId}/offers`);
  return response.data;
}

export async function acceptOffer(offerId: number): Promise<boolean> {
  try {
    await authAxios.put(`offer/${offerId}/accept`);
    return true;
  } catch (error: any) {
    const message =
      error?.response?.data?.error ||
      error?.message ||
      "An unknown error occurred.";
    console.error("Error:", message);
    return false;
  }
}

export async function rejectOffer(offerId: number): Promise<boolean> {
  try {
    await authAxios.put(`offer/${offerId}/reject`);
    return true;
  } catch (error: any) {
    const message =
      error?.response?.data?.error ||
      error?.message ||
      "An unknown error occurred.";
    console.error("Error:", message);
    return false;
  }
}

const AUCTION_HUB_URL = `${BACKEND_BASE_URL}/hubs/auction`;

type AuctionUpdateListener = (update: AuctionResponse) => void;
type OfferUpdateListener = (update: OfferResponse) => void;

class AuctionHubService {
  private connection: signalR.HubConnection | null = null;
  private startPromise: Promise<void> | null = null;
  private listeners = new Set<AuctionUpdateListener>();
  private offerUpdateListener: OfferUpdateListener | null = null;
  private joinedAuctionIds = new Set<number>();

  subscribe = (listener: AuctionUpdateListener) => {
    this.listeners.add(listener);
    return () => {
      this.listeners.delete(listener);
    };
  };

  subscribeOfferUpdate = (listener: OfferUpdateListener) => {
    this.offerUpdateListener = listener;
    return () => {
      this.offerUpdateListener = null;
    };
  };

  private emit(update: AuctionResponse) {
    this.listeners.forEach((listener) => listener(update));
  }

  private emitOfferUpdate(update: OfferResponse) {
    this.offerUpdateListener?.(update);
  }

  start(accessToken: string): Promise<void> {
    if (this.startPromise) return this.startPromise;

    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(AUCTION_HUB_URL, { accessTokenFactory: () => accessToken })
      .withAutomaticReconnect()
      .build();

    this.connection.on("ReceiveAuctionUpdate", (update: AuctionResponse) => {
      this.emit(update);
    });

    this.connection.on("ReceiveOfferUpdate", (update: OfferResponse) => {
      this.emitOfferUpdate(update);
    });

    this.connection.onreconnected(() => {
      this.joinedAuctionIds.forEach((auctionId) => {
        this.connection?.invoke("JoinAuction", auctionId).catch((error) => {
          console.error("Error rejoining auction:", error);
        });
      });
    });

    this.startPromise = this.connection.start().catch((error) => {
      console.error("Auction Connection Error:", error);
      this.startPromise = null;
    });

    return this.startPromise;
  }

  async joinAuction(auctionId: number) {
    this.joinedAuctionIds.add(auctionId);
    try {
      await this.connection?.invoke("JoinAuction", auctionId);
    } catch (error) {
      console.error("Error joining auction:", error);
    }
  }

  async leaveAuction(auctionId: number) {
    this.joinedAuctionIds.delete(auctionId);
    try {
      await this.connection?.invoke("LeaveAuction", auctionId);
    } catch (error) {
      console.error("Error leaving auction:", error);
    }
  }
}

export const auctionHubService = new AuctionHubService();
