import authAxios from "./authAxios";

export interface UpdateUserBiographyRequest {
  biography: string;
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
