import { Currency } from "./enums";

export interface AuctionStartDTO {
  startTime: string;
  endTime: string;
  startingPrice: number;
  currency: Currency;
}
