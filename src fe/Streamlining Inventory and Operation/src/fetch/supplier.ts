import { axiosInstanceJson } from "./axios-instance";

export const getListSuppliers = async (pageNumber: number, pageSize: number) => {
    try {
        const response = await axiosInstanceJson.get('/api/Supplier', {
            params: {
                pageNumber,
                pageSize,
            },
        });
        return response.data;
    } catch (error: any) {
        const errors = error.response?.data?.errorMessages || error.response?.data?.errors || [];
        return errors;
    }
};

// Lấy thông tin nhà cung cấp theo ID
export const getSupplierById = async (id: number) => {
    try {
        const response = await axiosInstanceJson.get(`/api/Supplier/${id}`);
        return response.data;
    } catch (error: any) {
        const errors = error.response?.data?.errorMessages || error.response?.data?.errors || [];
        return errors;
    };
};

// Kiểm tra xem nhà cung cấp có trùng tên, email, phone không
export const checkSupplierExists = async (submitData: { name: string, email: string, phone: string }) => {
    try {
        const response = await axiosInstanceJson.post('/api/Supplier/checkExists', submitData);
        return response.data.exists;  // Trả về true nếu trùng, false nếu không
    } catch (error: any) {
        const errors = error.response?.data?.errorMessages || error.response?.data?.errors || [];
        return errors;
    };
};

// Tạo nhà cung cấp mới
export const createSupplier = async (submitData: { name: string; email: string; phone: string }) => {
    try {
        const response = await axiosInstanceJson.post('/api/Supplier', submitData);
        return response.data;
    } catch (error: any) {
        const errors = error.response?.data?.errorMessages || error.response?.data?.errors || [];
        return errors;
    };
};

// Cập nhật nhà cung cấp
export const updateSupplierById = async (id: number, submitData: { name?: string; email?: string; phone?: string }) => {
    try {
        const response = await axiosInstanceJson.put(`/api/Supplier/${id}`, submitData);
        return response.data;
    } catch (error: any) {
        const errors = error.response?.data?.errorMessages || error.response?.data?.errors || [];
        return errors;
    };
};

// Xóa nhà cung cấp theo ID
export const deleteSupplierById = async (id: number) => {
    try {
        const response = await axiosInstanceJson.delete(`/api/Supplier/${id}`);
        return response.data;
    } catch (error: any) {
        const errors = error.response?.data?.errorMessages || error.response?.data?.errors || [];
        return errors;
    };
};


