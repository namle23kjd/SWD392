import { useEffect } from 'react'
import { useLocation, useNavigate } from 'react-router-dom'
import { getUserInfo } from '../util/auth'

export const useCheckRole = () => {
    const navigate = useNavigate()
    const location = useLocation()

    useEffect(() => {
        const userInfo = getUserInfo()
        const currentRoles = userInfo?.roles || []
        const currentPath = location.pathname

        if (currentPath.includes('/auth')) {
            return
        }

        // Nếu không có role, điều hướng về trang signin
        if (currentRoles.length === 0) {
            navigate('/auth/signin')
            return
        }

        // Kiểm tra xem đường dẫn có chứa /admin không và role có phải là admin không
        if (currentPath.includes('/admin') && !currentRoles.includes('Admin')) {
            navigate('/auth/signin')
            return
        }

        // Kiểm tra xem đường dẫn có chứa /manager không và role có phải là manager không
        if (currentPath.includes('/manager') && !currentRoles.includes('Manager')) {
            navigate('/auth/signin')
            return
        }

        // Kiểm tra xem đường dẫn có chứa /staff không và role có phải là staff không
        if (currentPath.includes('/staff') && !currentRoles.includes('Staff')) {
            navigate('/auth/signin')
            return
        }

        // Nếu role là admin và chưa vào /admin
        if (currentRoles.includes('Admin') && !currentPath.includes('/admin')) {
            navigate('/admin/accounts')
            return
        }

        // Nếu role là manager và chưa vào /manager
        if (currentRoles.includes('Manager') && !currentPath.includes('/manager')) {
            navigate('/manager/reports')
        }

        // Nếu role là staff và chưa vào /staff
        if (currentRoles.includes('Staff') && !currentPath.includes('/staff')) {
            navigate('/staff/products')
        }

        // Kiểm tra nếu đường dẫn không chứa /auth mà không có role thì điều hướng về /auth/signin
        if (!currentPath.includes('/auth') && !currentRoles) {
            navigate('/auth/signin')
        }
    }, [])
}
