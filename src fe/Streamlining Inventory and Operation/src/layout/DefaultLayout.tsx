import React, { useEffect, useState } from 'react';
import Header from '../components/Header/index';
import Sidebar from '../components/Sidebar/index';
import { Outlet, useNavigate } from 'react-router-dom';
import { getUserInfo } from '../util/auth';
import {useCheckRole} from "../hooks/"
const DefaultLayout: React.FC = () => {
  useCheckRole()
  const navigate = useNavigate();
  useEffect(() => {
    const userInfo = getUserInfo();
    console.log(userInfo);
    if (userInfo && userInfo?.role === 'admin') {
      navigate("/staff/reports");
    } else if (userInfo && userInfo?.role === 'manager') {
      navigate("/staff/reports");
    } else if (userInfo && userInfo?.role === 'staff') {
      navigate("/staff/reports");
    } else {
      navigate("/auth/signin");
    }
  }, [navigate]);

  const [sidebarOpen, setSidebarOpen] = useState(false);

  return (
    <div className="dark:bg-boxdark-2 dark:text-bodydark">
      {/* <!-- ===== Page Wrapper Start ===== --> */}
      <div className="flex h-screen overflow-hidden">
        {/* <!-- ===== Sidebar Start ===== --> */}
        <Sidebar sidebarOpen={sidebarOpen} setSidebarOpen={setSidebarOpen} />
        {/* <!-- ===== Sidebar End ===== --> */}

        {/* <!-- ===== Content Area Start ===== --> */}
        <div className="relative flex flex-1 flex-col overflow-y-auto overflow-x-hidden">
          {/* <!-- ===== Header Start ===== --> */}
          <Header sidebarOpen={sidebarOpen} setSidebarOpen={setSidebarOpen} />
          {/* <!-- ===== Header End ===== --> */}

          {/* <!-- ===== Main Content Start ===== --> */}
          <main>
            <div className="mx-auto max-w-screen-2xl p-4 md:p-6 2xl:p-10">
              <Outlet />
            </div>
          </main>
          {/* <!-- ===== Main Content End ===== --> */}
        </div>
        {/* <!-- ===== Content Area End ===== --> */}
      </div>
      {/* <!-- ===== Page Wrapper End ===== --> */}
    </div>
  );
};

export default DefaultLayout;
