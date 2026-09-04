import { useCallback, useEffect, useRef, useState } from "react";
import {
  MapContainer,
  TileLayer,
  AttributionControl,
  useMapEvents,
} from "react-leaflet";
import L from "leaflet";
import "leaflet/dist/leaflet.css";
import { toast } from "react-toastify";
import { useTranslation } from "react-i18next";
import i18n from "../i18n/i18n";
import Dock from "../components/Dock";
import GalleryMarker from "../components/map/GalleryMarker";
import GalleryMapSidebar from "../components/map/GalleryMapSidebar";
import "../styles/MapPage.css";
import { fetchMapPoints, GalleryMapPoint, MapBounds } from "../services/map";

const MIN_ZOOM_TO_FETCH = 6;
const WORLD_CENTER: [number, number] = [20, 0];
const WORLD_ZOOM = 2;
const VIEWPORT_DEBOUNCE_MS = 400;
const MAP_VIEWPORT_STORAGE_KEY = "mapViewport";

type SavedViewport = { lat: number; lng: number; zoom: number };

const getInitialViewport = (): SavedViewport => {
  try {
    const saved = sessionStorage.getItem(MAP_VIEWPORT_STORAGE_KEY);
    if (saved) return JSON.parse(saved);
  } catch (err) {
    toast.error(i18n.t("map.restoreViewportError"));
  }
  return { lat: WORLD_CENTER[0], lng: WORLD_CENTER[1], zoom: WORLD_ZOOM };
};

type ViewportWatcherProps = {
  onViewportChange: (
    bounds: MapBounds,
    zoom: number,
    center: { lat: number; lng: number },
  ) => void;
  onMapClick: () => void;
};

const readViewport = (map: L.Map) => {
  const bounds = map.getBounds();
  const center = map.getCenter();
  return {
    bounds: {
      south: bounds.getSouth(),
      west: bounds.getWest(),
      north: bounds.getNorth(),
      east: bounds.getEast(),
    },
    zoom: map.getZoom(),
    center: { lat: center.lat, lng: center.lng },
  };
};

const ViewportWatcher = ({
  onViewportChange,
  onMapClick,
}: ViewportWatcherProps) => {
  const debounceRef = useRef<ReturnType<typeof setTimeout> | null>(null);

  useEffect(() => {
    return () => {
      if (debounceRef.current) clearTimeout(debounceRef.current);
    };
  }, []);

  const reportViewport = useCallback(
    (map: L.Map) => {
      if (debounceRef.current) clearTimeout(debounceRef.current);
      debounceRef.current = setTimeout(() => {
        const { bounds, zoom, center } = readViewport(map);
        onViewportChange(bounds, zoom, center);
      }, VIEWPORT_DEBOUNCE_MS);
    },
    [onViewportChange],
  );

  const map = useMapEvents({
    moveend: (e) => reportViewport(e.target as L.Map),
    zoomend: (e) => reportViewport(e.target as L.Map),
    click: onMapClick,
  });

  useEffect(() => {
    const { bounds, zoom, center } = readViewport(map);
    onViewportChange(bounds, zoom, center);
  }, [map, onViewportChange]);

  return null;
};

const MapPage = () => {
  const { t } = useTranslation();
  const [initialViewport] = useState(getInitialViewport);
  const [points, setPoints] = useState<GalleryMapPoint[]>([]);
  const [loadingPoints, setLoadingPoints] = useState(false);
  const [zoom, setZoom] = useState(initialViewport.zoom);
  const requestIdRef = useRef(0);

  const [selectedGallery, setSelectedGallery] =
    useState<GalleryMapPoint | null>(null);

  const handleViewportChange = useCallback(
    async (
      bounds: MapBounds,
      currentZoom: number,
      center: { lat: number; lng: number },
    ) => {
      setZoom(currentZoom);
      sessionStorage.setItem(
        MAP_VIEWPORT_STORAGE_KEY,
        JSON.stringify({ lat: center.lat, lng: center.lng, zoom: currentZoom }),
      );

      if (currentZoom < MIN_ZOOM_TO_FETCH) {
        requestIdRef.current++;
        setPoints([]);
        return;
      }

      const requestId = ++requestIdRef.current;
      setLoadingPoints(true);
      try {
        const result = await fetchMapPoints(bounds);
        if (requestId === requestIdRef.current) {
          setPoints(result);
        }
      } catch (err) {
        console.error("Failed to fetch map points.");
      } finally {
        if (requestId === requestIdRef.current) {
          setLoadingPoints(false);
        }
      }
    },
    [],
  );

  const closeSidebar = useCallback(() => {
    setSelectedGallery(null);
  }, []);

  useEffect(() => {
    if (!selectedGallery) return;
    const handleKeyDown = (e: KeyboardEvent) => {
      if (e.key === "Escape") closeSidebar();
    };
    window.addEventListener("keydown", handleKeyDown);
    return () => window.removeEventListener("keydown", handleKeyDown);
  }, [selectedGallery, closeSidebar]);

  return (
    <div className="fixed-page">
      <div className="map-page">
        <MapContainer
          center={[initialViewport.lat, initialViewport.lng]}
          zoom={initialViewport.zoom}
          minZoom={2}
          worldCopyJump
          maxBounds={[
            [-90, -Infinity],
            [90, Infinity],
          ]}
          maxBoundsViscosity={1.0}
          attributionControl={false}
          className="map-page-container"
        >
          <TileLayer
            attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
            url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
          />
          <AttributionControl position="bottomleft" />
          <ViewportWatcher
            onViewportChange={handleViewportChange}
            onMapClick={closeSidebar}
          />

          {points.map((point) => (
            <GalleryMarker
              key={point.id}
              point={point}
              onSelect={setSelectedGallery}
            />
          ))}
        </MapContainer>

        {zoom < MIN_ZOOM_TO_FETCH && (
          <div className="map-zoom-hint">{t("map.zoomInHint")}</div>
        )}
        {loadingPoints && (
          <div className="map-loading-indicator">{t("map.loading")}</div>
        )}

        {selectedGallery && (
          <GalleryMapSidebar gallery={selectedGallery} onClose={closeSidebar} />
        )}

        <Dock />
      </div>
    </div>
  );
};

export default MapPage;
