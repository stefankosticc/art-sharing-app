import { FaLandmark } from "react-icons/fa6";
import "../../styles/GallerySearchCard.css";
import { FaCity } from "react-icons/fa";
import { Gallery } from "../../services/gallery";

type GallerySearchCardProps = {
  gallery: Gallery;
};

const GallerySearchCard = ({ gallery }: GallerySearchCardProps) => {
  if (!gallery) return null;

  return (
    <div className="gallery-sc-container">
      <div className="gallery-sc-icon-container">
        <FaLandmark />
      </div>
      <div className="gallery-sc-details">
        <p>{gallery.name}</p>
        <div className="asc-info">
          <span className="gallery-sc-info-with-icon">
            <FaCity />
            {gallery.cityName}
          </span>
        </div>
      </div>
    </div>
  );
};

export default GallerySearchCard;
