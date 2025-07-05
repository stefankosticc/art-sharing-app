import authAxios from "./authAxios";

export interface Gallery {
  id: number;
  name: string;
  address: string | null;
  cityId: number;
  cityName: string | null;
}

export async function searchGalleries(name: string): Promise<Gallery[]> {
  const response = await authAxios.get(`/galleries/search`, {
    params: { name },
  });
  return response.data;
}
