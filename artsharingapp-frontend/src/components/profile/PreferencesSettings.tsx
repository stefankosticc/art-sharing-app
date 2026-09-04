import { useState } from "react";
import { useTranslation } from "react-i18next";
import { Currency } from "../../services/enums";
import { getPreferredCurrency, savePreferredCurrency } from "../../utils/preferences";
import "./styles/PreferencesSettings.css";
import "./styles/SettingsModal.css";

const LANGUAGES: { code: string; label: string }[] = [
  { code: "en", label: "English" },
  { code: "sr", label: "Srpski" },
  { code: "es", label: "Español" },
];

const PreferencesSettings = () => {
  const { t, i18n } = useTranslation();
  const [currency, setCurrency] = useState<Currency>(getPreferredCurrency());

  const handleCurrencyChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const value = Number(e.target.value);
    setCurrency(value);
    savePreferredCurrency(value);
  };

  return (
    <div className="tab-panel">
      <h5>{t("profile.preferencesTitle")}</h5>
      <p>{t("profile.preferencesDescription")}</p>

      <div className="preferences-info">
        <label htmlFor="preferences-language" className="preferences-field">
          {t("profile.languageLabel")}
          <select
            id="preferences-language"
            value={i18n.language}
            onChange={(e) => i18n.changeLanguage(e.target.value)}
          >
            {LANGUAGES.map((lang) => (
              <option key={lang.code} value={lang.code}>
                {lang.label}
              </option>
            ))}
          </select>
        </label>

        <label htmlFor="preferences-currency" className="preferences-field">
          {t("common.currencyLabel")}
          <select
            id="preferences-currency"
            value={currency}
            onChange={handleCurrencyChange}
          >
            {Object.keys(Currency)
              .filter((key) => isNaN(Number(key)))
              .map((cur) => (
                <option key={cur} value={Currency[cur as keyof typeof Currency]}>
                  {cur}
                </option>
              ))}
          </select>
        </label>
      </div>
    </div>
  );
};

export default PreferencesSettings;
