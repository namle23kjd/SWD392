import axios from "axios";

// API Base URL cho Platform
const BASE_URL = "/api/Platform";

// Các kiểu dữ liệu cho Platform
export interface Platform {
    id: string;
    name: string;
    isActive: boolean;
}

export interface CreatePlatformDTO {
    name: string;
    isActive: boolean;
}

export interface UpdatePlatformDTO {
    name?: string;
    isActive?: boolean;
}

// Lấy danh sách tất cả nền tảng
export const getAllPlatforms = async (): Promise<Platform[]> => {
    try {
        const response = await axios.get(BASE_URL);
        return response.data; // Trả về danh sách nền tảng
    } catch (error) {
        console.error("Error fetching platforms:", error);
        throw new Error("Error fetching platforms");
    }
};

// Lấy thông tin nền tảng theo ID
export const getPlatformById = async (id: string): Promise<Platform> => {
    try {
        const response = await axios.get(`${BASE_URL}/${id}`);
        return response.data; // Trả về nền tảng theo ID
    } catch (error) {
        console.error(`Error fetching platform with ID ${id}:`, error);
        throw new Error("Error fetching platform");
    }
};

// Tạo nền tảng mới
export const createPlatform = async (platformData: CreatePlatformDTO): Promise<Platform> => {
    try {
        const response = await axios.post(BASE_URL, platformData);
        return response.data; // Trả về nền tảng mới được tạo
    } catch (error) {
        console.error("Error creating platform:", error);
        throw new Error("Error creating platform");
    }
};

// Cập nhật nền tảng
export const updatePlatform = async (id: string, platformData: UpdatePlatformDTO): Promise<Platform> => {
    try {
        const response = await axios.put(`${BASE_URL}/${id}`, platformData);
        return response.data; // Trả về nền tảng đã được cập nhật
    } catch (error) {
        console.error(`Error updating platform with ID ${id}:`, error);
        throw new Error("Error updating platform");
    }
};

// Xóa nền tảng theo ID
export const deletePlatform = async (id: string): Promise<void> => {
    try {
        await axios.delete(`${BASE_URL}/${id}`);
    } catch (error) {
        console.error(`Error deleting platform with ID ${id}:`, error);
        throw new Error("Error deleting platform");
    }
};
