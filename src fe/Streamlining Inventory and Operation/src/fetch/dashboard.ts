import { axiosInstanceJson } from "./axios-instance"

export const getOveralData = async () => {
    const data = await axiosInstanceJson.get('/api/Dashboard')
        .then((response) => {
            return response.data
        }
        ).catch((error) => {
            const errors = error.response.data.errorMessages
                || error.response.data.errors || []
            return errors
        })
    return data
}

export const getOrdersByPlatform = async () => {
    const data = await axiosInstanceJson.get('/api/Dashboard/orders-by-platform')
        .then((response) => {
            return response.data
        }
        ).catch((error) => {
            const errors = error.response.data.errorMessages
                || error.response.data.errors || []
            return errors
        })
    return data
}

export const getLowStockProducts = async () => {
    const data = await axiosInstanceJson.get('/api/Dashboard/low-stock-products')
        .then((response) => {
            return response.data
        }
        ).catch((error) => {
            const errors = error.response.data.errorMessages
                || error.response.data.errors || []
            return errors
        })
    return data
}

export const getSupplierImports = async () => {
    const data = await axiosInstanceJson.get('/api/Dashboard/all-suppliers')
        .then((response) => {
            return response.data
        }
        ).catch((error) => {
            const errors = error.response.data.errorMessages
                || error.response.data.errors || []
            return errors
        })
    return data
}

export const getTopProducts = async () => {
    const data = await axiosInstanceJson.get('/api/Dashboard/top-products')
        .then((response) => {
            return response.data
        }
        ).catch((error) => {
            const errors = error.response.data.errorMessages
                || error.response.data.errors || []
            return errors
        })
    return data
}