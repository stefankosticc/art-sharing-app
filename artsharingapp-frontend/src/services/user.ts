import authAxios from "./authAxios";

export interface UpdateUserBiographyRequest {
  biography: string;
}

export interface UserSearchResponse {
  id: number;
  name: string;
  userName: string;
}

export const updateUserBiography = async (
  request: UpdateUserBiographyRequest
): Promise<void> => {
  try {
    await authAxios.put(`/user/biography`, request);
  } catch (error: any) {
    const message =
      error?.response?.data?.error ||
      error?.message ||
      "An unknown error occurred while updating users biography.";
    console.error("Error:", message);
  }
};

export async function searchArtists(
  searchString: string
): Promise<UserSearchResponse[]> {
  const response = await authAxios.get(`/users/search`, {
    params: { searchString },
  });
  return response.data;
}
