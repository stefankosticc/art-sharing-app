import authAxios from "./authAxios";

export interface City {
  id: number;
  name: string;
  country: string | null;
}

export async function searchCities(name: string): Promise<City[]> {
  const response = await authAxios.get(`/cities/search`, {
    params: { name },
  });
  return response.data;
}
