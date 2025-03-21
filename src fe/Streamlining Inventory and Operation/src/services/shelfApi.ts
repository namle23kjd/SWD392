import { axiosInstanceJson } from "../fetch/axios-instance";
import { baseURL } from "./baseURL";

export const createShelfs = async (body: any) => {
    return axiosInstanceJson.post(`${baseURL}/Shelf`, body);
}
export const getShelfs = async (pageNumber:number, pageSize: number) => {
    return axiosInstanceJson.get(`${baseURL}/Shelf`,{
        params:{
            pageNumber,
            pageSize
        }
    });
}
export const getAllShelfs = async () =>{
    return axiosInstanceJson.get(`${baseURL}/Shelf`);
}
export const updateShelfs = async (shelfId: number, body: any) => {
    return axiosInstanceJson.put(`${baseURL}/Shelf/${shelfId}`, body);
}
export const deleteShelfs = async (shelfId: number) => {
    return axiosInstanceJson.delete(`${baseURL}/Shelf/${shelfId}`);
}