// src/columns/shelfColumns.ts
import { Space, TableColumnsType, TableProps, Tag } from 'antd';
import {
  DeleteOutlined,
  EditOutlined,
  ZoomInOutlined,
} from '@ant-design/icons';
import { Product } from '../pages/Products';

export const productColumns = (
  handleEdit: (record: Product) => void,
  handleSelectedDelete: (record: Product) => void,
  handleView: (record: Product) => void,
): TableColumnsType<Product> => [
  {
    title: 'Product Id',
    dataIndex: 'productId',
    key: 'productId',
    render: (text) => <a>{text}</a>,
    fixed: 'left',
  },
  {
    title: 'SKU',
    dataIndex: 'sku',
    key: 'sku',
    fixed: 'left',
  },
  {
    title: 'BarCode',
    dataIndex: 'barcode',
    key: 'barCode',
    fixed: 'left',
  },
  {
    title: 'Name',
    dataIndex: 'productName',
    key: 'name',
  },
  {
    title: 'Description',
    dataIndex: 'description',
    key: 'description',
  },
  {
    title: 'Base Price',
    dataIndex: 'basePrice',
    key: 'basePrice',
  },
  {
    title: 'Create At',
    dataIndex: 'createdAt',
    key: 'createdAt',
    render: (text) => <div className="text-green-700">{new Date(text).toLocaleString()}</div>,
  },
  {
    title: 'Update At',
    dataIndex: 'updatedAt',
    key: 'updatedAt',
    render: (text) => <div className="text-red">{new Date(text).toLocaleString()}</div>,
  },
  {
    title: 'User Name',
    dataIndex: 'userName',
    key: 'userName',
    render: (text) => <div className="text-blue-600">{text}</div>,
  },
  {
    title: 'Action',
    key: 'action',
    fixed: 'right',
    render: (_, record: Product) => (
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
