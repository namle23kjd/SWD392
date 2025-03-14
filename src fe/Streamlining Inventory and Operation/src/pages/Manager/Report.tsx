import React, { useEffect, useState } from 'react';
import Breadcrumb from '../../components/Breadcrumbs/Breadcrumb';
import CardDataStats from '../../components/CardDataStats';
import ChartOne from '../../components/Charts/ChartOne';
import ChartThree from '../../components/Charts/ChartThree';
import ChartTwo from '../../components/Charts/ChartTwo';
import MapOne from '../../components/Maps/MapOne';
import { getOveralData } from '../../fetch/dashboard';
import { AppstoreAddOutlined, ExportOutlined, ImportOutlined, UserOutlined } from '@ant-design/icons';

const Report: React.FC = () => {
    const [overalData, setOveralData] = useState({
        totalExports: 0,
        totalUsers: 0,
        totalImports: 0,
        totalQuantity: 0,
    })

    async function handleFetchOveralData() {
        const response = await getOveralData()
        setOveralData(response)
    }

    useEffect(() => {
        handleFetchOveralData()
    }, [])

    return (
        <>
            <Breadcrumb pageName='Report' />
            <div className="grid grid-cols-1 gap-4 md:grid-cols-2 md:gap-6 xl:grid-cols-4 2xl:gap-7.5">
                <CardDataStats title="Total Exports" total={overalData.totalExports.toString()}>
                    <ExportOutlined className="fill-primary dark:fill-white" style={{ fontSize: '22px' }} />
                </CardDataStats>

                <CardDataStats title="Total Users" total={overalData.totalUsers.toString()}>
                    <UserOutlined className="fill-primary dark:fill-white" style={{ fontSize: '22px' }} />
                </CardDataStats>

                <CardDataStats title="Total Imports" total={overalData.totalImports.toString()}>
                    <ImportOutlined className="fill-primary dark:fill-white" style={{ fontSize: '22px' }} />
                </CardDataStats>

                <CardDataStats title="Total Quantity" total={overalData.totalQuantity.toString()}>
                    <AppstoreAddOutlined className="fill-primary dark:fill-white" style={{ fontSize: '22px' }} />
                </CardDataStats>
            </div>

            <div className="mt-4 grid grid-cols-12 gap-4 md:mt-6 md:gap-6 2xl:mt-7.5 2xl:gap-7.5">
                <ChartOne />
                <ChartTwo />
                <ChartThree />
                <MapOne />
            </div>
        </>
    );
};

export default Report;
