import { Currency } from "../services/enums";

const PREFERRED_CURRENCY_KEY = "preferredCurrency";

export const getPreferredCurrency = (): Currency => {
  const stored = localStorage.getItem(PREFERRED_CURRENCY_KEY);
  return stored !== null ? Number(stored) : Currency.USD;
};

export const savePreferredCurrency = (currency: Currency) => {
  localStorage.setItem(PREFERRED_CURRENCY_KEY, String(currency));
};
