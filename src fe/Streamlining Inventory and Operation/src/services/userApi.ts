import { axiosInstanceJson } from "../fetch/axios-instance";
import { baseURL } from "./baseURL";

export const getAllUsers = async () =>{
    return axiosInstanceJson.get(`${baseURL}/Auth`);
}
export const resetPassword = async (email: string) =>{
    const body = {
        email: email,
        link: 'http://localhost:5173/auth/recover-password/reset?token=', 
      }
    return axiosInstanceJson.post(`${baseURL}/Auth/reset-password`, body);
}

export const resetConfirmResetPassword = async (body: any) =>{
    return axiosInstanceJson.post(`${baseURL}/Auth/confirm-reset-password`, body);
}