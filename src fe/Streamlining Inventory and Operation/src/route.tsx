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

export const router = createBrowserRouter([
    {
        path: '/',
        element: <>
            <ScrollToTop />
            <DefaultLayout />
        </>,
        errorElement: <ErrorPage />,
        children: [
            // role demo
            {
                path: '',
                children: [
                    {
                        index: true,
                        path: '',
                        element: (
                            <>
                                <PageTitle title="eCommerce Dashboard | TailAdmin - Tailwind CSS Admin Dashboard Template" />
                                <ECommerce />
                            </>
                        )
                    },
                    
                    {
                        path: 'order',
                        element: (
                            <>
                                <PageTitle title="Order Dashboard" />
                                <Order />
                            </>
                        )
                    }
                    , {
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
                    }, {
                        path: "orders",
                        element: (
                            <>
                                <PageTitle title="Order Manage" />
                                <Orders  />
                            </>
                        )
                    }, {
                        path: "profile",
                        element: (
                            <>
                                <PageTitle title="Profile | TailAdmin - Tailwind CSS Admin Dashboard Template" />
                                <Profile />
                            </>
                        )
                    },{
                        path: "tables",
                        element: (
                            <>
                                <PageTitle title="Tables | TailAdmin - Tailwind CSS Admin Dashboard Template" />
                                <Tables />
                            </>
                        )
                    }, {
                        path: "chart",
                        element: (
                            <>
                                <PageTitle title="Basic Chart | TailAdmin - Tailwind CSS Admin Dashboard Template" />
                                <Chart />
                            </>
                        )
                    },
                    {
                        path: 'forms',
                        children: [
                            {
                                path: "form-elements",
                                element: (<>
                                    <PageTitle title="Form Elements | TailAdmin - Tailwind CSS Admin Dashboard Template" />
                                    <FormElements />
                                </>)
                            }, {
                                path: "form-layout",
                                element: (
                                    <>
                                        <PageTitle title="Form Layout | TailAdmin - Tailwind CSS Admin Dashboard Template" />
                                        <FormLayout />
                                    </>
                                )
                            },
                        ]
                    },
                    {
                        path: 'ui',
                        children: [
                            {
                                path: "alerts",
                                element: (
                                    <>
                                        <PageTitle title="Alerts | TailAdmin - Tailwind CSS Admin Dashboard Template" />
                                        <Alerts />
                                    </>
                                )
                            }, {
                                path: "buttons",
                                element: (
                                    <>
                                        <PageTitle title="Buttons | TailAdmin - Tailwind CSS Admin Dashboard Template" />
                                        <Buttons />
                                    </>
                                )
                            },
                        ]
                    },
                ]
            }
            , {
                path: '/staff',
                element: <></>
            }, {
                path: '/admin',
                element: <></>
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
                    <PageTitle title="Signin | TailAdmin - Tailwind CSS Admin Dashboard Template" />
                    <SignIn />
                </>)
            }, {
                path: "signup",
                element: (<>
                    <PageTitle title="Sign Up | TailAdmin - Tailwind CSS Admin Dashboard Template" />
                    <SignUp />
                </>)
            }, {
                path: 'logout',
                action: logoutAction,
            }
        ]
    }
])