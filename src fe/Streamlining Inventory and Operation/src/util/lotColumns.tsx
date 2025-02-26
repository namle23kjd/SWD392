// src/columns/shelfColumns.ts

import { Space, TableColumnsType, TableProps, Tag } from 'antd';
import {
  DeleteOutlined,
  EditOutlined,
  ZoomInOutlined,
} from '@ant-design/icons';
import { Lot } from '../pages/Lots';

export const lotColumns = (
  handleEdit: (record: Lot) => void,
  handleSelectedDelete: (record: Lot) => void,
  handleView: (record: Lot) => void,
): TableColumnsType<Lot> => [
  { title: 'Id', dataIndex: 'lotId', key: 'lotId', fixed: 'left' },
  {
    title: 'Product Id',
    dataIndex: 'productId',
    key: 'productId',
    fixed: 'left',
  },
  {
    title: 'Shelf Id',
    dataIndex: 'shelfId',
    key: 'shelfId',
    fixed: 'left',
  },
  {
    title: 'Create By',
    dataIndex: 'userName',
    key: 'userName',
    fixed: 'left',
    render: (text) => <div className="text-blue-600">{text}</div>,
  },
  {
    title: 'Lot Code',
    dataIndex: 'lotCode',
    key: 'lotCode',
  },
  {
    title: 'Manafacture Date',
    dataIndex: 'manufactureDate',
    key: 'manufactureDate',
    render: (text) => <div className="text-green-800">{text}</div>,
  },
  {
    title: 'Expiry Date',
    dataIndex: 'expiryDate',
    key: 'expiryDate',
    render: (text) => <div className="text-red">{text}</div>,
  },
  {
    title: 'Quantity',
    dataIndex: 'quantity',
    key: 'quantity',
  },
  {
    title: 'Status',
    dataIndex: 'status',
    key: 'status',
    render: (status: string) => {
      let color = '';
      let text = '';

      switch (status) {
        case 'Active':
          color = 'green';
          text = 'Active';
          break;
        case 'Expired':
          color = 'volcano';
          text = 'Expired';
          break;
        case 'Empty':
          color = 'gray';
          text = 'Empty';
          break;
        default:
          color = 'default';
          text = status;
      }
      return <Tag color={color}>{text}</Tag>;
    },
  },
  {
    title: 'Create At',
    dataIndex: 'createdAt',
    key: 'createdAt',
    render: (text) => <div className="text-green">{text}</div>,
  },
  {
    title: 'Update At',
    dataIndex: 'updatedAt',
    key: 'updatedAt',
    render: (text) => <div className="text-yellow-600">{text}</div>,
  },
  {
    title: 'Action',
    key: 'action',
    fixed: 'right',
    render: (_, record: Lot) => (
      <Space size="middle">
        <EditOutlined
          onClick={() => handleEdit(record)}
          style={{ fontSize: 18, color: '#1890ff', cursor: 'pointer' }}
        />
        <DeleteOutlined
          onClick={() => handleSelectedDelete(record)}
          style={{ fontSize: 18, color: '#ff4d4f', cursor: 'pointer' }}
        />
        <ZoomInOutlined
          onClick={() => handleView(record)}
          style={{ fontSize: 18, color: '#52c41a', cursor: 'pointer' }}
        />
      </Space>
    ),
  },
];
