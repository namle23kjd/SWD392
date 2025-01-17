import { redirect } from 'react-router-dom'

export const getAuthToken = () => {
    const token = localStorage.getItem('token')
    if (!token) {
        return null
    }
    const tokenDuration = getAuthTokenDuration()

    if (tokenDuration < 0) {
        return 'EXPIRED'
    }
    return token
}

export const setAuthToken = (token: string) => {
    localStorage.setItem('token', token)
}

export const getAuthTokenDuration = () => {
    const storeExpirationDate = localStorage.getItem('expiration') ?? ''
    const expirationDate = new Date(storeExpirationDate)

    const now = new Date()
    const duration = expirationDate.getTime() - now.getTime()

    return duration
}

export const tokenLoader = () => {
    return getAuthToken()
}

export const checkAuthLoader = () => {
    const token = getAuthToken()

    if (!token) {
        return redirect('/auth')
    }

    return null
}
