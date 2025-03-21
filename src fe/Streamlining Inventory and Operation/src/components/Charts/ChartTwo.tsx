import { ApexOptions } from 'apexcharts';
import React, { useEffect, useState } from 'react';
import ReactApexChart from 'react-apexcharts';
import { getSupplierImports } from '../../fetch/dashboard';
import { useQuery } from '@tanstack/react-query';
import { Spin } from 'antd';

const colorPalette = [
  '#3C50E0', '#80CAEE', '#FF5733', '#FFBD33', '#FF8D33',
  '#FF33A1', '#33FF57', '#33D6FF', '#B033FF', '#FF5733'
];

const options: ApexOptions = {
  chart: {
    fontFamily: 'Satoshi, sans-serif',
    type: 'bar',
    height: 335,
    toolbar: {
      show: false,
    },
    zoom: {
      enabled: false,
    },
  },

  responsive: [
    {
      breakpoint: 1536,
      options: {
        plotOptions: {
          bar: {
            borderRadius: 0,
            columnWidth: '25%',
          },
        },
      },
    },
  ],
  plotOptions: {
    bar: {
      horizontal: false,
      borderRadius: 0,
      columnWidth: '50%', // Adjust column width
    },
  },
  dataLabels: {
    enabled: false,
  },

  xaxis: {
    categories: [], // We will populate this dynamically
  },
  legend: {
    position: 'top',
    horizontalAlign: 'left',
    fontFamily: 'Satoshi',
    fontWeight: 500,
    fontSize: '14px',

    markers: {
      radius: 99,
    },
  },
  fill: {
    opacity: 1,
  },
};

interface ChartTwoState {
  series: {
    name: string;
    data: number[];
  }[];
  categories: string[]; // To store supplier names for the x-axis
}

const ChartTwo: React.FC = () => {
  const [state, setState] = useState<ChartTwoState>({
    series: [
      {
        name: 'Total Import',
        data: [],
      },
    ],
    categories: [],
  });

  const { data, isPending } = useQuery({
    queryKey: ['supplier-import'],
    queryFn: () => getSupplierImports(),
  })

  async function handleFetchData() {
    if (data) {
      const seriesData = data.map((item: any) => item.totalImport);
      const categoriesData = data.map((item: any) => item.supplierName);

      // Create a color array based on the number of suppliers
      const colors = colorPalette.slice(0, Math.min(categoriesData.length, 10));

      setState({
        series: [
          {
            name: 'Total Import',
            data: seriesData,
          },
        ],
        categories: categoriesData, // Set supplier names here
      });

      // Dynamically update the color palette for the chart
      options.colors = colors;
    }
  }

  useEffect(() => {
    handleFetchData();
  }, [data]);

  return (
    <div className="col-span-12 rounded-sm border border-stroke bg-white p-7.5 shadow-default dark:border-strokedark dark:bg-boxdark xl:col-span-4">
      <div className="mb-4 justify-between gap-4 sm:flex">
        <div>
          <h4 className="text-xl font-semibold text-black dark:text-white">
            Supplier Imports
          </h4>
        </div>
      </div>

      <div>
        <div id="chartTwo" className="-ml-5 -mb-9">
          {isPending ? <Spin /> : <ReactApexChart
            options={{
              ...options,
              xaxis: { categories: state.categories }, // Dynamically set categories (supplier names)
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

export default ChartTwo;
