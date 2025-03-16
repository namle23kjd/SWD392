import { AppstoreAddOutlined, ExportOutlined, ImportOutlined, UserOutlined } from '@ant-design/icons';
import {
    useQuery,
} from '@tanstack/react-query';
import { Flex, Spin } from 'antd';
import React from 'react';
import Breadcrumb from '../../components/Breadcrumbs/Breadcrumb';
import CardDataStats from '../../components/CardDataStats';
import ChartOne from '../../components/Charts/ChartOne';
import ChartThree from '../../components/Charts/ChartThree';
import ChartTwo from '../../components/Charts/ChartTwo';
import MapOne from '../../components/Maps/MapOne';
import { getOveralData } from '../../fetch/dashboard';
const Report: React.FC = () => {
    const { data, isLoading } = useQuery({
        queryKey: ['dashboard'],
        queryFn: () => getOveralData()
    })

    return (
        <>
            <Breadcrumb pageName='Report' />
            <div className="grid grid-cols-1 gap-4 md:grid-cols-2 md:gap-6 xl:grid-cols-4 2xl:gap-7.5">
                {isLoading ? <Flex align="center" gap="middle">
                    <Spin size="small" />
                </Flex> : <>
                    <CardDataStats title="Total Exports" total={data.totalExports.toString()}>
                        <ExportOutlined className="fill-primary dark:fill-white" style={{ fontSize: '22px' }} />
                    </CardDataStats>

                    <CardDataStats title="Total Users" total={data.totalUsers.toString()}>
                        <UserOutlined className="fill-primary dark:fill-white" style={{ fontSize: '22px' }} />
                    </CardDataStats>

                    <CardDataStats title="Total Imports" total={data.totalImports.toString()}>
                        <ImportOutlined className="fill-primary dark:fill-white" style={{ fontSize: '22px' }} />
                    </CardDataStats>

                    <CardDataStats title="Total Quantity" total={data.totalQuantity.toString()}>
                        <AppstoreAddOutlined className="fill-primary dark:fill-white" style={{ fontSize: '22px' }} />
                    </CardDataStats>
                </>}
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
