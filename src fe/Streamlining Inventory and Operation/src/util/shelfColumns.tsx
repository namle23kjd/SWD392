// src/columns/shelfColumns.ts

import { Button, InputNumber, Space, TableProps, Tag } from 'antd';
import { Shelf } from '../pages/Shelfs';
import {
  DeleteOutlined,
  EditOutlined,
  SearchOutlined,
  ZoomInOutlined,
} from '@ant-design/icons';

export const shelfColumns = (
  handleEdit: (record: Shelf) => void,
  handleDelete: (record: Shelf) => void,
  handleView: (record: Shelf) => void,
): TableProps<Shelf>['columns'] => [
  {
    title: 'Id',
    dataIndex: 'id',
    key: 'id',
  },
  {
    title: 'Name',
    dataIndex: 'name',
    key: 'name',
  },
  {
    title: 'Code',
    dataIndex: 'code',
    key: 'code',
  },
  {
    title: 'Location',
    dataIndex: 'location',
    key: 'location',
  },
  {
    title: 'Capacity',
    dataIndex: 'capacity',
    key: 'capacity',
  },
  {
    title: 'Status',
    dataIndex: 'isActive',
    key: 'isActive',
    render: (isActive: boolean) => (
      <Tag color={isActive ? 'green' : 'volcano'}>
        {isActive ? 'Active' : 'Inactive'}
      </Tag>
    ),
  },
  {
    title: 'Create At',
    dataIndex: 'createAt',
    key: 'createAt',
    render: (text) => <div className="text-red">{text}</div>,
  },
  {
    title: 'Update At',
    dataIndex: 'updateAt',
    key: 'updateAt',
    render: (text) => <div className="text-red">{text}</div>,
  },
  {
    title: 'Action',
    key: 'action',
    render: (_, record: Shelf) => (
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
