import authAxios from "./authAxios";

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

export interface Artwork {
  id: number;
  title: string;
  story: string;
  image: string;
  date: Date;
  tipsAndTricks: string;
  isPrivate: boolean;
  isOnSale: boolean;
  createdByArtistId: number;
  createdByArtistUserName: string;
  postedByUserId: number;
  postedByUserName: string;
  cityId: number | null;
  cityName: string | null;
  galleryId: number | null;
  galleryName: string | null;
  isLikedByLoggedInUser: boolean | null;
  color: string | null;
}

export interface ArtworkRequest {
  title: string;
  story: string;
  date: Date | string;
  tipsAndTricks: string;
  isPrivate: boolean;
  createdByArtistId: number;
  postedByUserId: number;
  cityId: number | null;
  galleryId: number | null;
}

export interface ArtworkSearchResponse {
  id: number;
  title: string;
  image: string;
  isOnSale: boolean;
  postedByUserId: number;
  postedByUserName: string;
  cityId: number | null;
  cityName: string | null;
  country: string | null;
  galleryId: number | null;
  galleryName: string | null;
}

export async function getMyArtworks(): Promise<ArtworkCardData[]> {
  const response = await authAxios.get(`/artworks/mine`);
  return response.data;
}

export async function getFavoriteArtworks(
  userId: number
): Promise<FavoriteArtwork[]> {
  const response = await authAxios.get(`/user/${userId}/liked-artworks`);
  return response.data;
}

export async function getArtwork(artworkId: number): Promise<Artwork> {
  const response = await authAxios.get(`/artwork/${artworkId}`);
  return response.data;
}

export async function likeArtwork(artworkId: number): Promise<boolean> {
  try {
    await authAxios.post(`/artwork/${artworkId}/like`);
    return true;
  } catch (error: any) {
    const message =
      error?.response?.data?.error ||
      error?.message ||
      "An unknown error occurred.";
    console.error("Error:", message);
    return false;
  }
}

export async function dislikeArtwork(artworkId: number): Promise<void> {
  try {
    await authAxios.delete(`/artwork/${artworkId}/dislike`);
  } catch (error: any) {
    const message =
      error?.response?.data?.error ||
      error?.message ||
      "An unknown error occurred.";
    console.error("Error:", message);
  }
}

export async function updateArtwork(
  artworkId: number,
  request: ArtworkRequest,
  artworkImage: File | null
): Promise<void> {
  try {
    const formData = new FormData();
    for (const key in request) {
      const value = (request as any)[key];
      formData.append(key, value !== null ? value.toString() : "");
    }

    if (artworkImage) {
      formData.append("artworkImage", artworkImage);
    }

    await authAxios.put(`/artwork/${artworkId}`, formData, {
      headers: { "Content-Type": "multipart/form-data" },
    });
  } catch (error: any) {
    const message =
      error?.response?.data?.error ||
      error?.message ||
      "An unknown error occurred while updating artwork.";
    console.error("Error:", message);
  }
}

export async function addNewArtwork(
  artwork: ArtworkRequest,
  artworkImage: File
): Promise<void> {
  try {
    const formData = new FormData();
    for (const key in artwork) {
      const value = (artwork as any)[key];
      formData.append(key, value !== null ? value.toString() : "");
    }
    formData.append("artworkImage", artworkImage);

    await authAxios.post(`/artwork`, formData, {
      headers: { "Content-Type": "multipart/form-data" },
    });
  } catch (error: any) {
    const message =
      error?.response?.data?.error ||
      error?.message ||
      "An unknown error occurred.";
    console.error("Error:", message);
  }
}

export async function searchArtworks(
  title: string
): Promise<ArtworkSearchResponse[]> {
  const response = await authAxios.get(`/artworks/search`, {
    params: { title },
  });
  return response.data;
}

export async function getArtworkImage(imageUrl: string): Promise<string> {
  try {
    const response = await authAxios.get(imageUrl, { responseType: "blob" });
    const url = URL.createObjectURL(response.data);
    return url;
  } catch (error: any) {
    const message =
      error?.response?.data?.error ||
      error?.message ||
      "An unknown error occurred.";
    console.error("Error:", message);
    return "";
  }
}
