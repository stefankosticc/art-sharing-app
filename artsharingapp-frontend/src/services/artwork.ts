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

export interface FavoriteArtwork {
  userId: number;
  artworkId: number;
  artworkTitle: string | null;
  artworkImage: string | null;
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

export async function getFavoriteArtworks(
  userId: number
): Promise<FavoriteArtwork[]> {
  var accessToken = localStorage.getItem("accessToken");
  const response = await axios.get(
    `${API_BASE_URL}/user/${userId}/liked-artworks`,
    {
      headers: {
        Authorization: `Bearer ${accessToken}`,
      },
    }
  );
  return response.data;
}
