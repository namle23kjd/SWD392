import { useEffect } from "react"
import { getAuthToken, getAuthTokenDuration } from "../util/auth"
import { useSubmit } from "react-router-dom"

export const useAutoLogout = () => {
    const token = getAuthToken()
    const submit = useSubmit()

    // Tự động logout người dùng sau 30 phút
    useEffect(() => {
        if (!token) {
            return
        }

        if (token === 'EXPIRED') {
            submit(null, { action: '/auth/logout', method: 'POST' })
            return
        }

        const tokenDuration = getAuthTokenDuration()

        const timer = setTimeout(() => {
            submit(null, { action: '/auth/logout', method: 'POST' })
        }, tokenDuration)

        return () => clearTimeout(timer)
    }, [token, submit])
}