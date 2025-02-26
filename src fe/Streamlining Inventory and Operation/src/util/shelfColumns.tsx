// src/columns/shelfColumns.ts

import { Button, InputNumber, Space, TableColumnsType, TableProps, Tag } from 'antd';
import { Shelf } from '../pages/Shelfs';
import {
  DeleteOutlined,
  EditOutlined,
  SearchOutlined,
  ZoomInOutlined,
} from '@ant-design/icons';

export const shelfColumns = (
  handleEdit: (record: Shelf) => void,
  handleSelectedDelete: (record: Shelf) => void,
  handleView: (record: Shelf) => void,
): TableColumnsType<Shelf>  => [
  {
    title: 'Id',
    dataIndex: 'shelfId',
    key: 'shelfId',
    fixed: 'left',
  }
  ,
  {
    title: 'Create By',
    dataIndex: 'userName',
    key: 'userName',
    fixed: 'left',
    render: (text) => <div className="text-blue-600">{text}</div>,
  },
  {
    title: 'Code',
    dataIndex: 'code',
    key: 'code',
    fixed: 'left',
  },
  {
    title: 'Name',
    dataIndex: 'name',
    key: 'name',
    
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
    dataIndex: 'createdAt',
    key: 'createAt',
    render: (text) => <div className="text-green-700">{text}</div>,
  },
  {
    title: 'Update At',
    dataIndex: 'updatedAt',
    key: 'updateAt',
    render: (text) => <div className="text-red">{text}</div>,
  },
  {
    title: 'Action',
    key: 'action',
    fixed: 'right',
    render: (_, record: Shelf) => (
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
