import { ApexOptions } from 'apexcharts';
import React, { useEffect, useState } from 'react';
import ReactApexChart from 'react-apexcharts';
import { getOrdersByPlatform } from '../../fetch/dashboard';
import { useQuery } from '@tanstack/react-query';
import { Spin } from 'antd';

interface ChartThreeState {
  series: number[];
  labels: string[];
}

const ChartThree: React.FC = () => {
  const [state, setState] = useState<ChartThreeState>({
    series: [0, 0, 0, 0], // Default values
    labels: ['Desktop', 'Tablet', 'Mobile', 'Unknown'], // Default labels
  });

  const options: ApexOptions = {
    chart: {
      fontFamily: 'Satoshi, sans-serif',
      type: 'donut',
    },
    colors: ['#6577F3', '#8FD0EF', '#0FADCF', '#FF5733',
      '#33FF57', '#FF33A1', '#8B00FF', '#FF4500', '#FFD700', '#00CED1']
    , // Always defined
    labels: state.labels,
    legend: {
      show: false,
      position: 'bottom',
    },
    plotOptions: {
      pie: {
        donut: {
          size: '65%',
          background: 'transparent',
        },
      },
    },
    dataLabels: {
      enabled: false,
    },
    responsive: [
      {
        breakpoint: 2600,
        options: {
          chart: {
            width: 380,
          },
        },
      },
      {
        breakpoint: 640,
        options: {
          chart: {
            width: 200,
          },
        },
      },
    ],
  };

  const { data, isPending } = useQuery({
    queryKey: ['orders-by-platform'],
    queryFn: getOrdersByPlatform,
  })

  async function handleFetchData() {
    if (data) {
      const seriesData = data.map((data: any) => data.orders);
      const labelsData = data.map((data: any) => data.platform);

      setState({
        series: seriesData,
        labels: labelsData,
      });
    }
  }

  useEffect(() => {
    handleFetchData();
  }, [data]);

  return (
    <div className="sm:px-7.5 col-span-12 rounded-sm border border-stroke bg-white px-5 pb-5 pt-7.5 shadow-default dark:border-strokedark dark:bg-boxdark xl:col-span-5">
      <div className="mb-3 justify-between gap-4 sm:flex">
        <div>
          <h5 className="text-xl font-semibold text-black dark:text-white">
            Orders Each Platform
          </h5>
        </div>
      </div>

      <div className="mb-2">
        <div id="chartThree" className="mx-auto flex justify-center">
          {isPending ? <Spin /> : <ReactApexChart
            options={options}
            series={state.series}
            type="donut"
          />}
        </div>
      </div>

      <div className="-mx-8 flex flex-wrap items-center justify-center gap-y-3">
        {state.labels.map((label, index) => {
          return (
            <div key={index} className="sm:w-1/2 w-full px-8">
              <div className="flex w-full items-center">
                <span
                  className={`mr-2 block h-3 w-full max-w-3 rounded-full bg-[${['#6577F3', '#8FD0EF', '#0FADCF', '#FF5733', '#33FF57', '#FF33A1', '#8B00FF', '#FF4500', '#FFD700', '#00CED1']
                  [index]}]`}
                ></span>
                <p className="flex w-full justify-between text-sm font-medium text-black dark:text-white">
                  <span> {label} </span>
                  <span> {state.series[index]} </span>
                </p>
              </div>
            </div>
          )
        })}
      </div>
    </div>
  );
};

export default ChartThree;
