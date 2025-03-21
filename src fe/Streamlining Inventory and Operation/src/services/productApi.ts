import { axiosInstanceJson } from "../fetch/axios-instance";
import { baseURL } from "./baseURL";

export const createProducts = async (body: any) => {
    return axiosInstanceJson.post(`${baseURL}/Product`, body);
}
export const getProducts = async (pageNumber:number, pageSize: number) => {
    return axiosInstanceJson.get(`${baseURL}/Product`,{
        params:{
            pageName: pageNumber,
            pageSize
        }
    });
}
export const getAllProducts = async () =>{
    return axiosInstanceJson.get(`${baseURL}/Product`);
}
export const updateProducts = async (productId: number, body: any) => {
    return axiosInstanceJson.put(`${baseURL}/Product/${productId}`, body);
}
export const deleteProducts = async (productId: number) => {
    return axiosInstanceJson.delete(`${baseURL}/Product/${productId}`);
}
export const searchProducts = async (sku: string) => {
    return axiosInstanceJson.get(`${baseURL}/Product/search`, {
        params: {
            sku
        }
    });
}
export const createProductBySuppliers = async (body: any) => {
    return axiosInstanceJson.post(`${baseURL}/Product/createProductBySUppliers`,body);
}