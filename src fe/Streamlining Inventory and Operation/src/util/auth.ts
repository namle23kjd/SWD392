import { redirect } from 'react-router-dom'
import Cookies from 'js-cookie'

const COOKIE_EXPIRATION_DAYS = 1 / 24;

export const getAuthToken = () => {
  const token = Cookies.get('token')
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
  // Lưu token vào cookie với HttpOnly và Secure flags
  Cookies.set('token', token, {
    expires: COOKIE_EXPIRATION_DAYS,
    secure: true,
    sameSite: 'Strict',
  })
}

export const getUserInfo = () => {
  const userInfo = Cookies.get('userInfo') // Sử dụng cookies thay vì sessionStorage
  if (!userInfo) {
    return null
  }
  return JSON.parse(userInfo)
}

export const setUserInfoToStorage = (userInfo: object) => {
  // Lưu thông tin người dùng vào cookie
  Cookies.set('userInfo', JSON.stringify(userInfo), {
    expires: COOKIE_EXPIRATION_DAYS,
    secure: true,
    sameSite: 'Strict',
  })
}

export const getAuthTokenDuration = () => {
  const storeExpirationDate = Cookies.get('expiration') ?? ''
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
