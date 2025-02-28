import axios from 'axios';
import { getAuthToken } from '../util/auth';
import { jwtDecode } from 'jwt-decode';

const token = getAuthToken()

export const axiosInstanceJson = axios.create({
    baseURL: import.meta.env.VITE_API_URI || 'https://warehousemanagement-api-dev-bsg3fveyfaacdfe7.southeastasia-01.azurewebsites.net',
    timeout: 10000,
    headers: {
        'Content-Type': 'application/json',
        'Authorization': token ? `Bearer ${token}` : '',
    },
});


// Define the type for the decoded token
export interface DecodedToken {
    "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name": string;
    "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier": string;
    jti: string;
    "http://schemas.microsoft.com/ws/2008/06/identity/claims/role": string[];
    exp: number;
    iss: string;
    aud: string;
}

export const decodeToken = (token: string): DecodedToken | null => {
    try {
        const decoded: DecodedToken = jwtDecode(token);
        return decoded;
    } catch (error) {
        console.error("Error decoding token", error);
        return null;
    }
};
