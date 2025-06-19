import axios from "axios";

const API_BASE_URL = "http://localhost:5125/api";

export interface ArtworkCardData {
  id: number;
  title: string;
  image: string;
  isPrivate: boolean;
  postedByUserId: number;
  postedByUserName: string;
  date: Date;
}

export async function getMyArtworks(): Promise<ArtworkCardData[]> {
  var accessToken = localStorage.getItem("accessToken");
  const response = await axios.get(`${API_BASE_URL}/artworks/mine`, {
    headers: {
      Authorization: `Bearer ${accessToken}`,
    },
  });
  return response.data;
}
