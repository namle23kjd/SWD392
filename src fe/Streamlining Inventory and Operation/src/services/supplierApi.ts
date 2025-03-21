import { axiosInstanceJson } from "../fetch/axios-instance";
import { baseURL } from "./baseURL";

export const getAllSuppliers = async () =>{
    return axiosInstanceJson.get(`${baseURL}/Supplier`);
}