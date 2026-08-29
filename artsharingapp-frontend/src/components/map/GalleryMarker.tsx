import L from "leaflet";
import { Marker, Tooltip, useMap } from "react-leaflet";
import { renderToStaticMarkup } from "react-dom/server";
import { FaLandmark } from "react-icons/fa6";
import { GalleryMapPoint } from "../../services/map";
import { MAP_PIN_WRAPPER_CLASS } from "./mapUtils";
import "./styles/GalleryMarker.css";

const galleryBadgeSvg = renderToStaticMarkup(<FaLandmark />);

const GALLERY_PIN_TEARDROP_PATH =
  "M12 0C5.373 0 0 5.373 0 12c0 9 12 20 12 20s12-11 12-20C24 5.373 18.627 0 12 0z";

const CLICK_ZOOM = 15;

const GALLERY_ICON = L.divIcon({
  className: MAP_PIN_WRAPPER_CLASS,
  html: `<div class="map-gallery-pin-shape"><svg viewBox="0 0 24 32" width="24" height="32"><path d="${GALLERY_PIN_TEARDROP_PATH}" /></svg><div class="map-gallery-pin-icon">${galleryBadgeSvg}</div></div>`,
  iconSize: [24, 32],
  iconAnchor: [12, 32],
});

type GalleryMarkerProps = {
  point: GalleryMapPoint;
  onSelect: (gallery: GalleryMapPoint) => void;
};

const GalleryMarker = ({ point, onSelect }: GalleryMarkerProps) => {
  const map = useMap();

  return (
    <Marker
      position={[point.latitude, point.longitude]}
      icon={GALLERY_ICON}
      eventHandlers={{
        click: (e) => {
          L.DomEvent.stopPropagation(e);
          if (map.getZoom() < 12) {
            map.flyTo([point.latitude, point.longitude], CLICK_ZOOM, { duration: 0.75 });
          }
          onSelect(point);
        },
      }}
    >
      <Tooltip direction="top" offset={[0, -32]}>
        {point.name}
      </Tooltip>
    </Marker>
  );
};

export default GalleryMarker;
