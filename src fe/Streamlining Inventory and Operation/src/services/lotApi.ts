import { axiosInstanceJson } from "../fetch/axios-instance";
import { baseURL } from "./baseURL";

export const createLots = async (body: any) => {
    return axiosInstanceJson.post(`${baseURL}/Lot`, body);
}
export const getLots = async (pageNumber:number, pageSize: number) => {
    return axiosInstanceJson.get(`${baseURL}/Lot`,{
        params:{
            pageNumber,
            pageSize
        }
    });
}
export const getAllLots = async () =>{
    return axiosInstanceJson.get(`${baseURL}/Lot`);
}
export const updateLots = async (lotId: number, body: any) => {
    return axiosInstanceJson.put(`${baseURL}/Lot/${lotId}`, body);
}
export const deleteLots = async (lotId: number) => {
    return axiosInstanceJson.delete(`${baseURL}/Lot/${lotId}`);
}
