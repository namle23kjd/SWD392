import axios from "axios"
import { baseURL } from "./baseURL";

export const createProducts = async (body: any) => {
    return axios.post(`${baseURL}/Product`, body);
}
export const getProducts = async (pageNumber:number, pageSize: number) => {
    return axios.get(`${baseURL}/Product`,{
        params:{
            pageName: pageNumber,
            pageSize
        }
    });
}
export const getAllProducts = async () =>{
    return axios.get(`${baseURL}/Product`);
}
export const updateProducts = async (productId: number, body: any) => {
    return axios.put(`${baseURL}/Product/${productId}`, body);
}
export const deleteProducts = async (productId: number) => {
    return axios.delete(`${baseURL}/Product/${productId}`);
}