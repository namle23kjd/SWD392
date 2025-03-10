import { createBrowserRouter } from 'react-router-dom';
import DefaultLayout from './layout/DefaultLayout';
import ScrollToTop from './components/ScrollToTop';
import PageTitle from './components/PageTitle';
import ECommerce from './pages/Dashboard/ECommerce';
import Profile from './pages/Profile';
import Tables from './pages/Tables';
import Chart from './pages/Chart';
import FormElements from './pages/Form/FormElements';
import FormLayout from './pages/Form/FormLayout';
import Alerts from './pages/UiElements/Alerts';
import Buttons from './pages/UiElements/Buttons';
import SignIn from './pages/Authentication/SignIn';
import SignUp from './pages/Authentication/SignUp';
import { action as logoutAction } from './pages/Authentication/Logout'
import Order from './pages/Dashboard/Order';
import Shelfs from './pages/Shelfs';
import Lots from './pages/Lots';
import Products from './pages/Products';
import Orders from './pages/Orders';
import ErrorPage from './pages/Error/ErrorPage';
import ForgotPassword from './pages/Authentication/ForgotPassword';
import RecoverPassword from './pages/Authentication/RecoverPassword';
import ImportProduct from './pages/Manager/ImportProduct';
import ExportProduct from './pages/Manager/ExportProduct';
import Report from './pages/Manager/Report';
import TransactionManagement from './pages/Manager/TransactionManagement';
import InventoryTracking from './pages/Manager/InventoryTracking';
import ManageAccount from './pages/Admin/ListAccount';
import ListAccount from './pages/Admin/ListAccount';
import AddAccount from './pages/Admin/AddAccount';
import EditAccount from './pages/Admin/EditAccount';
import OrderHistory from './pages/OrderHistory';
import CreateOrder from './pages/CreateOrder';
import SuppliersPage from './pages/SuppliersPage';
import Platforms from './pages/Platform';

export const router = createBrowserRouter([
    {
        path: '/',
        element: <>
            <ScrollToTop />
            <DefaultLayout />
        </>,
        errorElement: <ErrorPage />,
        children: [
            {
                path: 'staff',
                children: [
                    {
                        path: "shelfs",
                        element: (
                            <>
                                <PageTitle title="Shelfs Manage" />
                                <Shelfs />
                            </>
                        )
                    }, {
                        path: "lots",
                        element: (
                            <>
                                <PageTitle title="Lots Manage" />
                                <Lots />
                            </>
                        )
                    }

                    , {
                        path: 'products',
                        element: (
                            <>
                                <PageTitle title="Product Manage" />
                                <Products />
                            </>
                        )
                    },
                    {
                        path: 'reports',
                        element: (
                            <>
                                <PageTitle title="Reports" />
                                <Report />
                            </>
                        )
                    },
                    {
                        path: 'import-product',
                        element: (
                            <>
                                <PageTitle title="Import Product" />
                                <ImportProduct />
                            </>
                        )
                    },
                    {
                        path: 'export-product',
                        element: (
                            <>
                                <PageTitle title="Export Product" />
                                <ExportProduct />
                            </>
                        )
                    },
                    {
                        path: 'transaction-management',
                        element: (
                            <>
                                <PageTitle title="Transaction Management" />
                                <TransactionManagement />
                            </>
                        )
                    },
                    {
                        path: 'confirm-stock-daily',
                        element: (
                            <>
                                <PageTitle title="Confirm Stock Daily" />
                                <InventoryTracking />
                            </>
                        )
                    },
                    {
                        path: 'order-history',
                        element: (
                            <>
                                <PageTitle title="Order History" />
                                <OrderHistory />
                            </>
                        )
                    },
                    {
                        path: 'create-order',
                        element: (
                            <>
                                <PageTitle title="Order History" />
                                <CreateOrder />
                            </>
                        )
                    },
                    {
                        path: 'suppliers-page',
                        element: (
                            <>
                                <PageTitle title="Suppliers Page" />
                                <SuppliersPage />
                            </>
                        )
                    },
                    {
                        path: 'platforms',
                        element: (
                            <>
                                <PageTitle title="platforms" />
                                <Platforms />
                            </>
                        )
                    }
                ]
            }, {
                path: 'admin',
                children: [
                    {
                        path: 'accounts',
                        element: (
                            <>
                                <PageTitle title="Account List" />
                                <ListAccount />
                            </>
                        )
                    },
                    {
                        path: 'add-account',
                        element: (
                            <>
                                <PageTitle title="Add new Account" />
                                <AddAccount />
                            </>
                        )
                    },
                    {
                        path: 'edit-account/:id',
                        element: (
                            <>
                                <PageTitle title="Edit Account" />
                                <EditAccount />
                            </>
                        )
                    },
                ]
            }, {
                path: 'manager',
                children: [

                ]
            }
            // other roles here...
        ]
    }, {
        path: '/auth',
        errorElement: <ErrorPage />,
        children: [
            {
                path: "signin",
                element: (<>
                    <PageTitle title="Signin | Warehouse management" />
                    <SignIn />
                </>)
            }, {
                path: "signup",
                element: (<>
                    <PageTitle title="Sign Up | Warehouse management" />
                    <SignUp />
                </>)
            }, {
                path: 'logout',
                action: logoutAction,
            }, {
                path: 'forgot-password',
                element: (<>
                    <PageTitle title="Forgot Password | Warehouse management" />
                    <ForgotPassword />
                </>)
            }
            , {
                path: 'recover-password',
                element: (<>
                    <PageTitle title="Recover Password | Warehouse management" />
                    <RecoverPassword />
                </>)
            }
        ]
    }
])