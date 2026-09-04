import authAxios from "./authAxios";

export interface MapBounds {
  south: number;
  west: number;
  north: number;
  east: number;
}

export interface GalleryMapPoint {
  id: number;
  name: string;
  address: string | null;
  cityId: number;
  cityName: string | null;
  latitude: number;
  longitude: number;
}

export async function fetchMapPoints(
  bounds: MapBounds,
): Promise<GalleryMapPoint[]> {
  const response = await authAxios.get("/galleries/map", { params: bounds });
  return response.data;
}
