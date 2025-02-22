import React, { useEffect, useState } from 'react';
import Breadcrumb from '../components/Breadcrumbs/Breadcrumb';
import { useNavigate } from 'react-router-dom';
import { Button, Collapse, DatePicker, Form, Input, InputNumber, Select, Table } from 'antd';
import { toast } from 'react-toastify';
import { lotColumns } from '../util/lotColumns';
import '../css/buttonSearch.css';
import { LAYOUT_LOT, VALIDATE_MESSAGES } from '../validate/validateMessages';
import { useStyle } from '../css/useStyle';

export interface Lot {
  id: number;
  productId: number;
  shelfId: number;
  lotCode: string;
  manafactureDate: string;
  expiryDate: string;
  quantity: number;
  status: string;
  createAt: string;
  updateAt: string;
  createBy: string;
}

const Lots: React.FC = () => {
  const navigate = useNavigate();
  const { Search } = Input;
  const { Panel } = Collapse;
  const [openModal, setOpenModal] = useState<boolean>(false);
  const [searchLoading, setSearchLoading] = useState<boolean>(false);
  const [activeKey, setActiveKey] = useState<string[]>([]);
  const [lots, setLots] = useState<Lot[]>([]);
  const [shelfIds, setShelfIds] = useState<number[]>([]);
  const [productIds, setProductIds] = useState<number[]>([]);
  const { styles } = useStyle();
  const dateFormat = 'DD-MM-YYYY';
  useEffect(() => {
    const fecthLots = async () => {
      try {
        const fetchLots: Lot[] = [
          {
            id: 1,
            productId: 101,
            shelfId: 1,
            lotCode: 'L001',
            manafactureDate: '2024-01-10',
            expiryDate: '2025-01-10',
            quantity: 100,
            status: 'Active',
            createAt: new Date().toISOString(),
            updateAt: new Date().toISOString(),
            createBy: 'Admin',
          },
          {
            id: 2,
            productId: 102,
            shelfId: 2,
            lotCode: 'L002',
            manafactureDate: '2024-02-15',
            expiryDate: '2025-02-15',
            quantity: 150,
            status: 'Active',
            createAt: new Date().toISOString(),
            updateAt: new Date().toISOString(),
            createBy: 'Admin',
          },
          {
            id: 3,
            productId: 103,
            shelfId: 3,
            lotCode: 'L003',
            manafactureDate: '2024-03-20',
            expiryDate: '2025-03-20',
            quantity: 200,
            status: 'Inactive',
            createAt: new Date().toISOString(),
            updateAt: new Date().toISOString(),
            createBy: 'Admin',
          },
          {
            id: 4,
            productId: 104,
            shelfId: 4,
            lotCode: 'L004',
            manafactureDate: '2024-04-10',
            expiryDate: '2025-04-10',
            quantity: 250,
            status: 'Active',
            createAt: new Date().toISOString(),
            updateAt: new Date().toISOString(),
            createBy: 'Admin',
          },
        ];
        console.log('fecthlots', fetchLots);
        setLots(fetchLots);
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
    const fetchProductIds = async () => {
      try {
        const fetchProductIds: number[] = [101, 102, 103, 104]; // Replace with actual API call
        setProductIds(fetchProductIds);
      } catch (error) {
        toast.error('Failed to fetch product IDs');
      }
    };

    fetchShelfIds();
    fetchProductIds();
    fecthLots();
  }, []);

  const handleSearch = (record: any) => {
    setSearchLoading(true);
    console.log('search Lot', record);
    setSearchLoading(false);
  };
  const handleEdit = (record: Lot) => {
    console.log('edit Lot', record);
  };
  const handleDelete = (record: Lot) => {
    console.log('delete Lot', record);
  };
  const handleView = (record: Lot) => {
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
  
      <Breadcrumb pageName="Lots" />
      <Collapse  activeKey={activeKey} onChange={toggleCollapse} style={{ marginBottom: '20px' }}>
          <Panel header="Create New Lot" key="1">
            <div className="bg-white p-8 rounded-md w-full">
              <Form
                {...LAYOUT_LOT}
                name="lots"
                onFinish={onFinish}
                style={{ maxWidth: 600 }}
                validateMessages={VALIDATE_MESSAGES}
              >
                <Form.Item
                  name={['lotCode']}
                  label="Lot Code"
                  rules={[{ required: true }]}
                >
                  <Input />
                </Form.Item>
                <Form.Item
                name={['shelfId']}
                label="Shelf ID"
                rules={[{ required: true }]}
              >
                <Select>
                  {shelfIds.map(id => (
                    <Select.Option key={id} value={id}>
                      {id}
                    </Select.Option>
                  ))}
                </Select>
              </Form.Item>
              <Form.Item
                name={['productId']}
                label="Product ID"
                rules={[{ required: true }]}
              >
                <Select>
                  {productIds.length > 0 ? (
                    productIds.map(id => (
                      <Select.Option key={id} value={id}>
                        {id}
                      </Select.Option>
                    ))
                  ) : (
                    <Select.Option value={null}>None</Select.Option>
                  )}
                </Select>
              </Form.Item>

                <Form.Item
                  name={['managefactureDate']}
                  label="Managefacture Date"
                  rules={[{ required: true, type: 'object' }]}
                >
                   <DatePicker format={dateFormat} />
                </Form.Item>
                <Form.Item
                  name={['expiryDate']}
                  label="Expiry Date"
                  rules={[{ required: true, type: 'object' }]}
                >
                  <DatePicker format={dateFormat} />
                </Form.Item>
                <Form.Item
                  name={['quantity']}
                  label="Quantity"
                  rules={[{ type: 'number', required: true, min: 0, max: 200 }]}
                >
                  <InputNumber />
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
          placeholder="Search by Code"
          enterButton="Search"
          size="large"
          loading={searchLoading}
          onSearch={handleSearch}
          className="custom-search-button"
          style={{ width: '50%' }}
        />
      </div>
      <div>
        <Table<Lot>
          className={styles.customTable}
          rowKey="id"
          pagination={{ pageSize: 2 }}
          columns={lotColumns(handleEdit, handleDelete, handleView)}
          dataSource={lots}
          scroll={{ x: 'max-content' }}
        />
      </div>
    </>
  );
};

export default Lots;
