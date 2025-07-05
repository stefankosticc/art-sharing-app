import "../../styles/ArtworkSearchCard.css";
import { FaLandmark } from "react-icons/fa6";
import { FaCity } from "react-icons/fa";
import { ArtworkSearchResponse } from "../../services/artwork";
import { useNavigate } from "react-router-dom";

const fallbackImage =
  "https://cdn.shopify.com/s/files/1/0047/4231/6066/files/The_Scream_by_Edvard_Munch_1893_800x.png";
//   "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSxUJETfjcCv4VMKnbIpdK32QicBPC-dF17Fg&s";
//   "https://artrkl.com/cdn/shop/articles/thecreationofadam-1690035964350_d2d6280f-ed1d-465e-ad42-0ea0bbbcefde.webp?v=1690563054&width=1100";

type ArtworkSearchCardProps = {
  artwork: ArtworkSearchResponse;
};

const ArtworkSearchCard = ({ artwork }: ArtworkSearchCardProps) => {
  const navigate = useNavigate();

  if (!artwork) return null;

  return (
    <div
      className="asc-container"
      onClick={() => navigate(`/artwork/${artwork.id}`)}
    >
      <div className="asc-img-container">
        <img
          src={artwork.image || fallbackImage}
          alt={artwork.title}
          onError={(e) => {
            (e.target as HTMLImageElement).src = fallbackImage;
          }}
        />
      </div>
      <div className="asc-details">
        <p>{artwork.title}</p>
        <div className="asc-info">
          <span>{"@" + artwork.postedByUserName}</span>
          {artwork.cityName && (
            <span className="asc-info-with-icon">
              <FaCity />
              {artwork.cityName + ", " + artwork.country}
            </span>
          )}
          {artwork.galleryName && (
            <span className="asc-info-with-icon">
              <FaLandmark />
              {artwork.galleryName}
            </span>
          )}
        </div>
      </div>
      {artwork.isOnSale && <div className="asc-on-sale">ON SALE</div>}
    </div>
  );
};

export default ArtworkSearchCard;
