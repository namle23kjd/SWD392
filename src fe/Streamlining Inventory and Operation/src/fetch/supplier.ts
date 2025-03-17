import { axiosInstanceJson } from "./axios-instance";

// Lấy danh sách nhà cung cấp
export const getListSuppliers = async () => {
    try {
        const response = await axiosInstanceJson.get('/api/Supplier');
        return response.data;
    } catch (error: any) {
        console.error("Error fetching suppliers:", error);
        if (error.response) {
            throw new Error(`API Error: ${error.response.data.message || error.response.statusText}`);
        }
        throw new Error("Network or server error occurred while fetching suppliers.");
    }
};

// Lấy thông tin nhà cung cấp theo ID
export const getSupplierById = async (id: number) => {
    try {
        const response = await axiosInstanceJson.get(`/api/Supplier/${id}`);
        return response.data;
    } catch (error: any) {
        console.error(`Error fetching supplier ${id}:`, error);
        if (error.response) {
            throw new Error(`API Error: ${error.response.data.message || error.response.statusText}`);
        }
        throw new Error("Network or server error occurred while fetching supplier details.");
    }
};

// Kiểm tra xem nhà cung cấp có trùng tên, email, phone không
export const checkSupplierExists = async (submitData: { name: string, email: string, phone: string }) => {
    try {
        const response = await axiosInstanceJson.post('/api/Supplier/checkExists', submitData);
        return response.data.exists;  // Trả về true nếu trùng, false nếu không
    } catch (error: any) {
        console.error("Error checking supplier existence:", error);
        if (error.response) {
            throw new Error(`API Error: ${error.response.data.message || error.response.statusText}`);
        }
        throw new Error("Network or server error occurred while checking supplier existence.");
    }
};

// Tạo nhà cung cấp mới
export const createSupplier = async (submitData: { name: string; email: string; phone: string; createdAt: string }) => {
    try {
        const response = await axiosInstanceJson.post('/api/Supplier', submitData);
        return response.data;
    } catch (error: any) {
        console.error("Error creating supplier:", error);
        if (error.response) {
            throw new Error(`API Error: ${error.response.data.message || error.response.statusText}`);
        }
        throw new Error("Network or server error occurred while creating supplier.");
    }
};

// Cập nhật nhà cung cấp
export const updateSupplierById = async (id: number, submitData: { name?: string; email?: string; phone?: string; createdAt?: string }) => {
    try {
        const response = await axiosInstanceJson.put(`/api/Supplier/${id}`, submitData);
        return response.data;
    } catch (error: any) {
        console.error(`Error updating supplier ${id}:`, error);
        if (error.response) {
            throw new Error(`API Error: ${error.response.data.message || error.response.statusText}`);
        }
        throw new Error("Network or server error occurred while updating supplier.");
    }
};

// Xóa nhà cung cấp theo ID
export const deleteSupplierById = async (id: number) => {
    try {
        const response = await axiosInstanceJson.delete(`/api/Supplier/${id}`);
        return response.data;
    } catch (error: any) {
        console.error(`Error deleting supplier ${id}:`, error);
        if (error.response) {
            throw new Error(`API Error: ${error.response.data.message || error.response.statusText}`);
        }
        throw new Error("Network or server error occurred while deleting supplier.");
    }
};


