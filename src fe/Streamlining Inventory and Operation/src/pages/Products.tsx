import React, { useEffect, useState } from 'react';
import Breadcrumb from '../components/Breadcrumbs/Breadcrumb';
import {
  Button,
  Collapse,
  DatePicker,
  Form,
  Input,
  InputNumber,
  Select,
  Table,
} from 'antd';
import { useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';
import { LAYOUT_LOT, VALIDATE_MESSAGES } from '../validate/validateMessages';
import { productColumns } from '../util/productColumns';

export interface Product {
  id: number;
  sku: string;
  barcode: string;
  name: string;
  description: string;
  basePrice: number;
  createAt: string;
  updateAt: string;
  createById: string;
}

const Products = () => {
  const navigate = useNavigate();
  const { Search } = Input;
  const { Panel } = Collapse;
  const [openModal, setOpenModal] = useState<boolean>(false);
  const [searchLoading, setSearchLoading] = useState<boolean>(false);
  const [activeKey, setActiveKey] = useState<string[]>([]);
  const [products, setProducts] = useState<Product[]>([]);
  const [shelfIds, setShelfIds] = useState<number[]>([]);
  const [lotIds, setLotIds] = useState<number[]>([]);
  const dateFormat = 'DD-MM-YYYY';

  useEffect(() => {
    const fetchProducts = async () => {
      try {
        const fetchProducts: Product[] = [
          {
            id: 1,
            sku: 'P001',
            barcode: '1234567890123',
            name: 'Product A',
            description: 'This is product A description.',
            basePrice: 100.0,
            createAt: new Date().toISOString(),
            updateAt: new Date().toISOString(),
            createById: 'Admin',
          },
          {
            id: 2,
            sku: 'P002',
            barcode: '1234567890124',
            name: 'Product B',
            description: 'This is product B description.',
            basePrice: 150.0,
            createAt: new Date().toISOString(),
            updateAt: new Date().toISOString(),
            createById: 'Admin',
          },
          {
            id: 3,
            sku: 'P003',
            barcode: '1234567890125',
            name: 'Product C',
            description: 'This is product C description.',
            basePrice: 200.0,
            createAt: new Date().toISOString(),
            updateAt: new Date().toISOString(),
            createById: 'Admin',
          },
          {
            id: 4,
            sku: 'P004',
            barcode: '1234567890126',
            name: 'Product D',
            description: 'This is product D description.',
            basePrice: 250.0,
            createAt: new Date().toISOString(),
            updateAt: new Date().toISOString(),
            createById: 'Admin',
          },
        ];
        console.log('fecthlots', fetchProducts);
        setProducts(fetchProducts);
      } catch (error) {
        toast.error('Failed to fetch lots');
      }
    };
    const fetchShelfIds = async () => {
      try {
        const fetchShelfIds: number[] = [1, 2, 3, 4]; // Replace with actual API call
        setShelfIds(fetchShelfIds);
      } catch (error) {
        toast.error('Failed to fetch shelf IDs');
      }
    };
    const fetchLotIds = async () => {
      try {
        const fetchLotIds: number[] = [12, 43, 2, 12]; // Replace with actual API call
        setLotIds(fetchLotIds);
      } catch (error) {
        toast.error('Failed to fetch product IDs');
      }
    };

    fetchShelfIds();
    fetchLotIds();
    fetchProducts();
  }, []);

  const handleSearch = (record: any) => {
    setSearchLoading(true);
    console.log('search Lot', record);
    setSearchLoading(false);
  };
  const handleEdit = (record: Product) => {
    console.log('edit Lot', record);
  };
  const handleDelete = (record: Product) => {
    console.log('delete Lot', record);
  };
  const handleView = (record: Product) => {
    console.log('view Lot', record);
  };

  const onFinish = (values: any) => {
    console.log(values);
    setOpenModal(false);
  };
  const toggleCollapse = () => {
    console.log('activeKey', activeKey);
    console.log('activeKey', activeKey.length);
    setActiveKey(activeKey.length ? [] : ['1']);
  };

  return (
    <>
      <Breadcrumb pageName="Product Manage" />
      <Collapse
        activeKey={activeKey}
        onChange={toggleCollapse}
        style={{ marginBottom: '20px' }}
      >
        <Panel header="Create New Product" key="1">
          <div className="bg-white p-8 rounded-md w-full">
            <Form
              {...LAYOUT_LOT}
              name="products"
              onFinish={onFinish}
              style={{ maxWidth: 600 }}
              validateMessages={VALIDATE_MESSAGES}
            >
              <Form.Item
                name={['sku']}
                label="SKU"
                rules={[{ required: true }]}
              >
                <Input />
              </Form.Item>
              <Form.Item
                name={['barcode']}
                label="BarCode"
                rules={[{ required: true }]}
              >
                <Input />
              </Form.Item>
              <Form.Item
                name={['name']}
                label="Product Name"
                rules={[{ required: true }]}
              >
                <Input />
              </Form.Item>
              <Form.Item
                name={['description']}
                label="Description"
                rules={[{ required: true }]}
              >
                <Input />
              </Form.Item>
              <Form.Item
                name={['basePrice']}
                label="Base Price"
                rules={[{ required: true }]}
              >
                <InputNumber min={0} step={0.01} />
              </Form.Item>
              <Form.Item
                name={['shelfId']}
                label="Shelf ID"
                rules={[{ required: true }]}
              >
                <Select>
                  {shelfIds.map((id) => (
                    <Select.Option key={id} value={id}>
                      {id}
                    </Select.Option>
                  ))}
                </Select>
              </Form.Item>
              <Form.Item
                name={['lotId']}
                label="Lot ID"
                rules={[{ required: true }]}
              >
                <Select>
                  {lotIds.map((id) => (
                    <Select.Option key={id} value={id}>
                      {id}
                    </Select.Option>
                  ))}
                </Select>
              </Form.Item>

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
            </Form>
          </div>
        </Panel>
      </Collapse>
      <div
        style={{
          display: 'flex',
          justifyContent: 'flex-end',
          marginBottom: '20px',
        }}
      >
        <Search
          placeholder="Search by Bar Code"
          enterButton="Search"
          size="large"
          loading={searchLoading}
          onSearch={handleSearch}
          className="custom-search-button"
          style={{ width: '50%' }}
        />
      </div>
      <Table
        rowKey="id"
        pagination={{ pageSize: 2 }}
        columns={productColumns(handleEdit, handleDelete, handleView)}
        dataSource={products}
      />
    </>
  );
};

export default Products;
