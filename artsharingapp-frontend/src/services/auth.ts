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
export interface SignUpRequest {
    name: string;
    email: string;
    userName: string;
    password: string;
}

export interface User {
    id: number;
    name: string;
    email: string;
    userName: string;
    biography?: string;
    roleId: number;
    roleName?: string;
}

export async function login(request: LoginRequest): Promise<LoginResponse> {
    const response = await axios.post(`${API_BASE_URL}/auth/login`, request);
    return response.data;
}

export async function signUp(request: SignUpRequest): Promise<void> {
    await axios.post(`${API_BASE_URL}/auth/register`, request);
}

export async function getLoggedInUser(): Promise<User> {
    var accessToken = localStorage.getItem("accessToken");
    const response = await axios.get(`${API_BASE_URL}/auth/loggedin-user`, {
        headers: {
            Authorization: `Bearer ${accessToken}`,
        },
    });
    return response.data;
}
