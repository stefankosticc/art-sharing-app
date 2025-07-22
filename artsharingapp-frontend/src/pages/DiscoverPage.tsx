import { FiSearch } from "react-icons/fi";
import Dock from "../components/Dock";
import TopArtistsSection from "../components/TopArtistsSection";
import "../styles/DiscoverPage.css";

const DiscoverPage = () => {
  return (
    <div className="fixed-page">
      <div className="discover-page">
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
