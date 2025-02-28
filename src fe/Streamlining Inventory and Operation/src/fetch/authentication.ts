import { axiosInstanceJson } from "./axios-instance"
export const loginAction = async (username: string, password: string): Promise<any> => {
    const data = await axiosInstanceJson.post('/api/Auth/login', {
        username,
        password
    }).then((response) => {
        return response.data
    }
    ).catch((error) => {
        const errors = error.response.data.errorMessages || error.response.data.errors || []
        return errors
    })

    return data
}