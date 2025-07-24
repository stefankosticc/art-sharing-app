import Dock from "../components/Dock";
import TopArtistsSection from "../components/TopArtistsSection";
import "../styles/DiscoverPage.css";
import { NavLink } from "react-router-dom";

const DiscoverPage = () => {
  return (
    <div className="fixed-page">
      <div className="discover-page">
        <div className="discover-nav-container">
          <NavLink to={"/discover"} className={"active"}>
            Discover
          </NavLink>
          <NavLink to={"/following"}>Following</NavLink>
        </div>
        <h1>Fresh Finds</h1>
        <div className="dp-content">
          <TopArtistsSection />
        </div>
      </div>

      <Dock />
    </div>
  );
};

export default DiscoverPage;
