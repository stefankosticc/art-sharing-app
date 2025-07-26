import authAxios from "./authAxios";

export async function followUser(userId: number): Promise<boolean> {
  try {
    await authAxios.post(`/follow/${userId}`);
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

export async function unfollowUser(userId: number): Promise<void> {
  try {
    await authAxios.delete(`/unfollow/${userId}`);
  } catch (error: any) {
    const message =
      error?.response?.data?.error ||
      error?.message ||
      "An unknown error occurred.";
    console.error("Error:", message);
  }
}
