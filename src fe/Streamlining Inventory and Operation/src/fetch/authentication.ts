import { axiosInstanceJson } from "./axios-instance"

export const loginAction = (username: string, password: string) => {
    axiosInstanceJson.post('/api/Auth/login', {
        username,
        password
    }).then((response) => {
        console.log(response.data);
    }
    ).catch((error) => {
        console.log(error);
    })
}