import { useEffect, useState } from "react";
import { getLowStockProducts } from "../../fetch/dashboard";
import ReactApexChart from 'react-apexcharts';
import { ApexOptions } from "apexcharts";
import { useQuery } from "@tanstack/react-query";
import { Spin } from "antd";

const colorPalette = [
  '#3C50E0', '#80CAEE', '#FF5733', '#FFBD33', '#FF8D33',
  '#FF33A1', '#33FF57', '#33D6FF', '#B033FF', '#FF5733'
];

const MapOne = () => {
  const [chartData, setChartData] = useState({
    categories: [], series: [{
      name: 'Products in Stock',
      data: [] as number[],
    }]
  });

  const { data, isPending } = useQuery({
    queryKey: ['lowstock-products'],
    queryFn: () => getLowStockProducts(),
  })

  async function fetchData() {
    if (data) {
      const categories = data.map((item: any) => item.lot);
      const series = data.map((item: any) => item.products);
      setChartData({
        categories,
        series: [{
          name: 'Products in Stock',
          data: series,
        }]
      });
    }

  }

  useEffect(() => {
    fetchData();
  }, [data]);

  const options: ApexOptions = {
    chart: {
      type: 'bar',
      height: 350,
    },
    xaxis: {
      categories: chartData.categories, // Map lots to the x-axis
    },
    title: {
      text: 'Low Stock Products per Lot',
      align: 'center',
      style: {
        fontSize: '18px',
        fontWeight: 'bold',
      },
    },
    dataLabels: {
      enabled: false,
    },
    fill: {
      opacity: 1,
    },
    colors: colorPalette.slice(0, chartData.categories.length), // Dynamically assign colors for each bar
  };

  return (
    <div className="col-span-12 rounded-sm border border-stroke bg-white py-6 px-7.5 shadow-default dark:border-strokedark dark:bg-boxdark xl:col-span-7">
      <h4 className="mb-2 text-xl font-semibold text-black dark:text-white">
        Low Stock Products
      </h4>
      <div id="chart">
        {isPending ? <Spin /> : <ReactApexChart
          options={options}
          series={chartData.series}
          type="bar"
          height={350}
        />}
      </div>
    </div>
  );
};

export default MapOne;
