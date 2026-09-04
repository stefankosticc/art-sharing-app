import React from "react";
import { NavLink } from "react-router-dom";
import { useTranslation } from "react-i18next";

const DiscoverNavbar = () => {
  const { t } = useTranslation();

  return (
    <div className="discover-nav-container">
      <NavLink to={"/discover"}>{t("discover.nav.discover")}</NavLink>
      <NavLink to={"/following"}>{t("discover.nav.following")}</NavLink>
      <NavLink to={"/active-auctions"}>{t("discover.nav.auctions")}</NavLink>
    </div>
  );
};

export default DiscoverNavbar;
