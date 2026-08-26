import React from "react";
import { NavLink } from "react-router-dom";

const DiscoverNavbar = () => {
  return (
    <div className="discover-nav-container">
      <NavLink to={"/discover"}>Discover</NavLink>
      <NavLink to={"/following"}>Following</NavLink>
      <NavLink to={"/active-auctions"}>Auctions</NavLink>
    </div>
  );
};

export default DiscoverNavbar;
