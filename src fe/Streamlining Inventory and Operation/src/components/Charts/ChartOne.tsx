import { ApexOptions } from 'apexcharts';
import React, { useEffect, useState } from 'react';
import ReactApexChart from 'react-apexcharts';
import { getTopProducts } from '../../fetch/dashboard';
import { useQuery } from '@tanstack/react-query';
import { Spin } from 'antd';

const colorPalette = ['#6577F3', '#8FD0EF', '#0FADCF', '#FF5733', '#33FF57', '#FF33A1',
  '#8B00FF', '#FF4500', '#FFD700', '#00CED1'];

const options: ApexOptions = {
  legend: {
    show: false,
    position: 'top',
    horizontalAlign: 'left',
  },
  chart: {
    fontFamily: 'Satoshi, sans-serif',
    height: 335,
    type: 'bar', // Column chart type
    toolbar: {
      show: false,
    },
  },
  responsive: [
    {
      breakpoint: 1024,
      options: {
        chart: {
          height: 300,
        },
      },
    },
    {
      breakpoint: 1366,
      options: {
        chart: {
          height: 350,
        },
      },
    },
  ],
  grid: {
    xaxis: {
      lines: {
        show: true,
      },
    },
    yaxis: {
      lines: {
        show: true,
      },
    },
  },
  dataLabels: {
    enabled: false,
  },
  markers: {
    size: 4,
    colors: '#fff',
    strokeWidth: 3,
    strokeOpacity: 0.9,
    fillOpacity: 1,
  },
  xaxis: {
    categories: [], // Will be populated with product names
  },
  yaxis: {
    title: {
      style: {
        fontSize: '0px',
      },
    },
    min: 0,
  },
};

interface ChartOneState {
  series: {
    name: string;
    data: number[];
  }[];
  categories: string[]; // For storing product names
  totalRevenue: number; // For displaying total revenue dynamically
  totalSales: number; // For displaying total sales dynamically
  colors: string[]; // For storing dynamically generated colors for each product
}

const ChartOne: React.FC = () => {
  const [state, setState] = useState<ChartOneState>({
    series: [],
    categories: [],
    totalRevenue: 0,
    totalSales: 0,
    colors: [],
  });

  const { data, isPending } = useQuery({
    queryKey: ['top-product'],
    queryFn: () => getTopProducts()
  })

  async function fetchData() {
    if (data) {
      const seriesData = data.map((item: any) => item.totalSold);
      const categoriesData = data.map((item: any) => item.productName);

      // Calculate total revenue and total sales
      const totalSales = seriesData.reduce((acc: any, curr: any) => acc + curr, 0);
      const totalRevenue = totalSales * 100; // Example: assume each product sold generates 100 revenue

      // Create a color array based on the number of products
      const colors = colorPalette.slice(0, Math.min(categoriesData.length, 10));

      setState({
        series: [
          {
            name: 'Total Sold',
            data: seriesData,
          },
        ],
        categories: categoriesData, // Set product names as categories
        totalRevenue: totalRevenue,
        totalSales: totalSales,
        colors: colors, // Set dynamic colors for each product
      });

      options.colors = colors; // Apply color palette to the chart
    }
  }

  useEffect(() => {
    fetchData();
  }, [data]);

  return (
    <div className="col-span-12 rounded-sm border border-stroke bg-white px-5 pt-7.5 pb-5 shadow-default dark:border-strokedark dark:bg-boxdark sm:px-7.5 xl:col-span-8">
      <div className="mb-4 justify-between gap-4 sm:flex">
        <div>
          <h4 className="text-xl font-semibold text-black dark:text-white">
            Top Products
          </h4>
        </div>
      </div>
      <div className="flex flex-wrap items-start justify-between gap-3 sm:flex-nowrap">
        <div className="flex w-full flex-wrap gap-3 sm:gap-5">
          <div className="flex min-w-47.5">
            <span className="mt-1 mr-2 flex h-4 w-full max-w-4 items-center justify-center rounded-full border border-secondary">
              <span className="block h-2.5 w-full max-w-2.5 rounded-full bg-primary"></span>
            </span>
            <div className="w-full">
              <p className="font-semibold text-primary">Total Sales: {state.totalSales}</p>
            </div>
          </div>
        </div>
      </div>

      <div>
        <div id="chartOne" className="-ml-5">
          {isPending ? <Spin /> : <ReactApexChart
            options={{
              ...options,
              xaxis: { categories: state.categories }, // Dynamically set categories (product names)
            }}
            series={state.series}
            type="bar"
            height={350}
          />}
        </div>
      </div>
    </div>
  );
};

export default ChartOne;
