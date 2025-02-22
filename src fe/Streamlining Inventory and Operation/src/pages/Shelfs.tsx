import React, { useEffect, useState } from 'react';
import Breadcrumb from '../components/Breadcrumbs/Breadcrumb';
import { useNavigate } from 'react-router-dom';
import {
  DeleteOutlined,
  EditOutlined,
  PlusOutlined,
  ZoomInOutlined,
} from '@ant-design/icons';
import { toast } from 'react-toastify';
import { Button, Form, Input, InputNumber, Table } from 'antd';
import { shelfColumns } from '../util/shelfColumns';
import { LAYOUT_SHELF, VALIDATE_MESSAGES } from '../validate/validateMessages';
import '../css/buttonSearch.css';
export interface Shelf {
  id: number;
  code: string;
  name: string;
  location: string;
  capacity: number;
  isActive: boolean;
  createAt: string;
  updateAt: string;
  createBy: string;
}

const Shelfs: React.FC = () => {
  const { Search } = Input;
  const navigate = useNavigate();
  const [openModal, setOpenModal] = useState<boolean>(false);
  const [shelfs, setShelfs] = useState<Shelf[]>([]);
  const [searchLoading, setSearchLoading] = useState<boolean>(false);
  useEffect(() => {
    const fecthShelfs = async () => {
      try {
        const fetchShelfs = [
          {
            id: 1,
            code: 'S001',
            name: 'Aba',
            location: 'A1',
            capacity: 10,
            isActive: true, // Add a default value for isActive
            createAt: new Date().toISOString(), // Add a value for createAt
            updateAt: new Date().toISOString(), // Add a value for updateAt
            createBy: 'Admin', // Add a value for createBy
          },
          {
            id: 2,
            code: 'S002',
            name: 'Anta',
            location: 'A2',
            capacity: 10,
            isActive: true,
            createAt: new Date().toISOString(),
            updateAt: new Date().toISOString(),
            createBy: 'Admin',
          },
          {
            id: 3,
            code: 'S003',
            name: 'Befa',
            location: 'A3',
            capacity: 10,
            isActive: true,
            createAt: new Date().toISOString(),
            updateAt: new Date().toISOString(),
            createBy: 'Admin',
          },
          {
            id: 4,
            code: 'S004',
            name: 'Naga',
            location: 'A4',
            capacity: 10,
            isActive: true,
            createAt: new Date().toISOString(),
            updateAt: new Date().toISOString(),
            createBy: 'Admin',
          },
        ];
        console.log('fetchShelfs', fetchShelfs);
        setShelfs(fetchShelfs);
      } catch (error) {
        toast.error('Failed to fetch shelfs');
      }
    };
    fecthShelfs();
  }, []);
  const handleSearch = (record: any) => {
      setSearchLoading(true);
      console.log('search shelf', record);
      setSearchLoading(false);
  };
  const handleEdit = (record: Shelf) => {
    console.log('edit shelf', record);
  };
  const handleDelete = (record: Shelf) => {
    console.log('delete shelf', record);
  };
  const handleView = (record: Shelf) => {
    console.log('view shelf', record);
  };

  const onFinish = (values: any) => {
    console.log(values);
    setOpenModal(false);
  };

  return (
    <>
      <Breadcrumb pageName="Shelfs Manage" />
      <div style={{ display: 'flex', justifyContent: 'flex-end', marginBottom: '20px' }}>
        <Search
          placeholder="Search By Code"
          enterButton="Search"
          size="large"
          loading={searchLoading}
          onSearch={handleSearch}
          className="custom-search-button"
          style={{ width: '50%' }}
        />
      </div>
      <div>
        <PlusOutlined
          onClick={() => setOpenModal(true)}
          className="mb-4 p-2 bg-blue-500 text-white rounded hover:-translate-y-1 hover:shadow-lg transition-transform"
        />
 
        <Table
          rowKey="id"
          pagination={{ pageSize: 2 }}
          columns={shelfColumns(handleEdit, handleDelete, handleView)}
          dataSource={shelfs}
        />
      </div>
      {openModal && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex justify-center items-center">
          <div className="bg-white p-8 rounded-md w-1/3">
            <h2 className="text-center text-2xl font-bold mb-8">Create New Shelf</h2>
            <Form
              {...LAYOUT_SHELF}
              name="shelfs"
              onFinish={onFinish}
              style={{ maxWidth: 600 }}
              validateMessages={VALIDATE_MESSAGES}
            >
              <Form.Item
                name={['code']}
                label="Code"
                rules={[{ required: true }]}
              >
                <Input />
              </Form.Item>
              <Form.Item
                name={['name']}
                label="Name"
                rules={[{ required: true }]}
              >
                <Input />
              </Form.Item>
              <Form.Item
                name={['location']}
                label="Location"
                rules={[{ required: true }]}
              >
                <Input />
              </Form.Item>
              <Form.Item
                name={['capacity']}
                label="Capacity"
                rules={[{ type: 'number', required: true, min: 0, max: 99 }]}
              >
                <InputNumber />
              </Form.Item>
              <div className="flex justify-center">
                <Form.Item label={null}>
                  <Button
                    type="primary"
                    htmlType="submit"
                    style={{
                      backgroundColor: '#1890ff',
                      borderColor: '#1890ff',
                      color: '#fff',
                      marginRight: '20px',
                    }}
                  >
                    Submit
                  </Button>
                </Form.Item>
                <Form.Item label={null}>
                  <Button
                    type="primary"
                    onClick={() => setOpenModal(false)}
                    style={{
                      backgroundColor: 'red',
                      borderColor: 'red',
                      color: '#fff',
                    }}
                    htmlType="button"
                  >
                    Close
                  </Button>
                </Form.Item>
              </div>
            </Form>
          </div>
        </div>
      )}
    </>
  );
};

export default Shelfs;
