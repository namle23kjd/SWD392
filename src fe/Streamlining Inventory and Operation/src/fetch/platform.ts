import { axiosInstanceJson } from "./axios-instance";

const BASE_URL = "/api/Platform";

// Lấy danh sách tất cả nền tảng
export const getAllPlatforms = async () => {
    try {
        const response = await axiosInstanceJson.get(BASE_URL);
        return response.data; // Trả về danh sách nền tảng
    } catch (error: any) {
        const errors = error.response?.data?.errorMessages || error.response?.data?.errors || [];
        return errors;
    };
};

// Lấy thông tin nền tảng theo ID
export const getPlatformById = async (id: string) => {
    try {
        const response = await axiosInstanceJson.get(`${BASE_URL}/${id}`);
        return response.data; // Trả về nền tảng theo ID
    } catch (error: any) {
        const errors = error.response?.data?.errorMessages || error.response?.data?.errors || [];
        return errors;
    };
};

// Tạo nền tảng mới
export const createPlatform = async (platformData: any) => {
    try {
        const response = await axiosInstanceJson.post(BASE_URL, platformData);
        return response.data; // Trả về nền tảng mới được tạo
    } catch (error: any) {
        const errors = error.response?.data?.errorMessages || error.response?.data?.errors || [];
        return errors;
    };
};

// Cập nhật nền tảng
export const updatePlatform = async (id: string, platformData: any) => {
    try {
        const response = await axiosInstanceJson.put(`${BASE_URL}/${id}`, platformData);
        return response.data; // Trả về nền tảng đã được cập nhật
    } catch (error: any) {
        const errors = error.response?.data?.errorMessages || error.response?.data?.errors || [];
        return errors;
    };
};

// Xóa nền tảng theo ID
export const deletePlatform = async (id: string) => {
    try {
        await axiosInstanceJson.delete(`${BASE_URL}/${id}`);
    } catch (error: any) {
        const errors = error.response?.data?.errorMessages || error.response?.data?.errors || [];
        return errors;
    };
};
