// src/columns/shelfColumns.ts
import { Space, TableProps, Tag } from 'antd';
import {
  DeleteOutlined,
  EditOutlined,
  ZoomInOutlined,
} from '@ant-design/icons';
import { Product } from '../pages/Products';

export const productColumns = (
  handleEdit: (record: Product) => void,
  handleDelete: (record: Product) => void,
  handleView: (record: Product) => void,
): TableProps<Product>['columns'] => [
  {
    title: 'Product Id',
    dataIndex: 'id',
    key: 'id',
    render: (text) => <a>{text}</a>,
  },
  {
    title: 'SKU',
    dataIndex: 'sku',
    key: 'sku',
  },
  {
    title: 'BarCode',
    dataIndex: 'barcode',
    key: 'barCode',
  },
  {
    title: 'Name',
    dataIndex: 'name',
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
    dataIndex: 'createAt',
    key: 'createAt',
    render: (text) => <div className="text-green-700">{text}</div>,
  },
  {
    title: 'Update At',
    dataIndex: 'updateAt',
    key: 'updateAt',
    render: (text) => <div className="text-red">{text}</div>,
  },
  {
    title: 'Create By Id',
    dataIndex: 'createById',
    key: 'createById',
    render: (text) => <div className="text-blue-600">{text}</div>,
  },
  {
    title: 'Action',
    key: 'action',
    render: (_, record: Product) => (
      <Space size="middle">
        <EditOutlined
          onClick={() => handleEdit(record)}
          style={{ fontSize: 18, color: '#1890ff', cursor: 'pointer' }}
        />
        <DeleteOutlined
          onClick={() => handleDelete(record)}
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
