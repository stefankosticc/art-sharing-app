import axios from "axios";

const API_BASE_URL = "http://localhost:5125/api";

export interface LoginRequest {
    email: string;
    password: string;
}

export interface LoginResponse {
    accessToken: string;
    refreshToken: string;
}

export async function login(request: LoginRequest): Promise<LoginResponse> {
    const response = await axios.post(`${API_BASE_URL}/auth/login`, request);
    return response.data;
}



