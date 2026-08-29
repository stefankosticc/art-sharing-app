import { IoCloseCircleOutline } from "react-icons/io5";
import { FaLandmark } from "react-icons/fa6";
import { useNavigate } from "react-router-dom";
import { GalleryMapPoint } from "../../services/map";
import { ARTWORK_FALLBACK_IMAGE, IMAGE_SERVICE_BASE_URL } from "../../config/constants";
import { useGalleryArtworks } from "../../hooks/useGalleryArtworks";
import "./styles/GalleryMapSidebar.css";

type GalleryMapSidebarProps = {
  gallery: GalleryMapPoint;
  onClose: () => void;
};

const GalleryMapSidebar = ({ gallery, onClose }: GalleryMapSidebarProps) => {
  const navigate = useNavigate();
  const { galleryArtworks, loadingGalleryArtworks } = useGalleryArtworks(
    gallery.id,
  );

  return (
    <div className="map-sidebar">
      <IoCloseCircleOutline className="map-sidebar-close" onClick={onClose} />

      <div className="ms-header">
        <div className="ms-icon-wrapper">
          <FaLandmark />
          <span>{gallery.name}</span>
        </div>
      </div>

      <div className="ms-content">
        <div className="ms-info">
          {gallery.cityName && (
            <p
              className="ms-city-name"
              onClick={() => navigate(`/city/${gallery.cityId}`)}
            >
              {gallery.cityName}
            </p>
          )}
          {gallery.address && <p>{gallery.address}</p>}
        </div>

        <button
          className="map-sidebar-view-gallery"
          onClick={() => navigate(`/gallery/${gallery.id}`)}
        >
          View gallery page
        </button>

        <h4 className="map-sidebar-artworks-heading">Artworks</h4>
        <div className="ms-artwork-grid">
          {loadingGalleryArtworks && (
            <div className="ms-artworks-loading">
              <div className="loading-spinner" />
            </div>
          )}
          {!loadingGalleryArtworks && galleryArtworks?.length === 0 && (
            <p className="map-sidebar-empty">
              No artworks in this gallery yet.
            </p>
          )}
          {!loadingGalleryArtworks &&
            galleryArtworks?.map((artwork) => (
              <div
                key={artwork.id}
                className="ms-artwork-card"
                onClick={() => navigate(`/artwork/${artwork.id}`)}
              >
                <img
                  src={`${IMAGE_SERVICE_BASE_URL}${artwork.image}`}
                  alt={artwork.title}
                  onError={(e) => {
                    e.currentTarget.src = ARTWORK_FALLBACK_IMAGE;
                  }}
                />
                <p>{artwork.title}</p>
              </div>
            ))}
        </div>
      </div>
    </div>
  );
};

export default GalleryMapSidebar;
