import axios from "axios";
import { baseURL } from "./baseURL";

export const getAllUsers = async () =>{
    return axios.get(`${baseURL}/Auth`);
}
export const resetPassword = async (email: string) =>{
    const body = {
        email: email,
        link: 'http://localhost:5173/auth/recover-password/reset?token=', 
      }
    return axios.post(`${baseURL}/Auth/reset-password`, body);
}

export const resetConfirmResetPassword = async (body: any) =>{
    return axios.post(`${baseURL}/Auth/confirm-reset-password`, body);
}