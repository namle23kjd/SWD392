import { redirect } from 'react-router-dom'

export function action() {
    localStorage.removeItem('userInfo')
    localStorage.removeItem('token')
    localStorage.removeItem('expiration')
    return redirect('/auth/signin')
}