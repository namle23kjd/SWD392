import { EditOutlined } from '@ant-design/icons';
import { Col, Modal, Row, Select, Table } from "antd";
import React, { useEffect, useState } from "react";
import { toast } from "react-toastify"; // For displaying error messages

const { Option } = Select;

const OrderHistory: React.FC = () => {
    // State to store order data and selected status
    const [orders, setOrders] = useState<any[]>([]);
    const [searchTerm, setSearchTerm] = useState<string>(''); // State for search term
    const [editedOrders, setEditedOrders] = useState<any[]>([]); // Store orders with edited statuses
    const [isModalVisible, setIsModalVisible] = useState(false); // Modal visibility
    const [selectedOrder, setSelectedOrder] = useState<any>(null); // Selected order for editing

    // Fetching orders data (simulated, replace with actual API call)
    useEffect(() => {
        const fetchOrders = async () => {
            try {
                const fetchedOrders = [
                    {
                        orderId: "ORD001",
                        platform: "Platform A",
                        status: "In Progress",
                        orderDate: "2025-02-19",
                        products: [
                            { productId: "P001", name: "Product A", quantity: 1, price: 100 },
                            { productId: "P002", name: "Product B", quantity: 2, price: 200 },
                        ],
                        notes: "Urgent",
                    },
                    {
                        orderId: "ORD002",
                        platform: "Platform B",
                        status: "Completed",
                        orderDate: "2025-02-18",
                        products: [
                            { productId: "P003", name: "Product C", quantity: 1, price: 150 },
                        ],
                        notes: "Standard delivery",
                    },
                ];
                setOrders(fetchedOrders);
                setEditedOrders(fetchedOrders);
            } catch (error) {
                toast.error("Failed to fetch orders.");
            }
        };

        fetchOrders();
    }, []);

    // Handle search functionality
    const handleSearch = (e: React.ChangeEvent<HTMLInputElement>) => {
        setSearchTerm(e.target.value);
    };

    // Filter orders based on the search term
    const filteredOrders = orders.filter(order =>
        order.orderId.toLowerCase().includes(searchTerm.toLowerCase()) ||
        order.platform.toLowerCase().includes(searchTerm.toLowerCase())
    );

    // Function to show the modal and set the selected order
    const showModal = (order: any) => {
        setSelectedOrder(order);
        setIsModalVisible(true);
    };

    // Function to handle modal save
    const handleModalSave = () => {
        const updatedOrders = editedOrders.map(order =>
            order.orderId === selectedOrder.orderId ? selectedOrder : order
        );
        setEditedOrders(updatedOrders);
        setIsModalVisible(false);
        toast.success("Order status updated successfully!");
    };

    // Function to handle modal cancel
    const handleModalCancel = () => {
        setIsModalVisible(false);
    };

    // Calculate total price of the order based on products
    const calculateTotalPrice = (products: any[]) => {
        return products.reduce((total, product) => total + (product.quantity * product.price), 0);
    };

    return (
        <div className="p-6 bg-gray-50">
            <h3 className="text-xl font-semibold text-gray-900">Order History</h3>

            {/* Search Bar */}
            <div className="mt-4 mb-6 flex justify-between items-center">
                <input
                    type="text"
                    placeholder="Search by Order ID or Platform"
                    className="p-2 border rounded-lg w-1/3"
                    value={searchTerm}
                    onChange={handleSearch}
                />
            </div>

            <div className="overflow-x-auto">
                <table className="min-w-full bg-white shadow-md rounded-lg">
                    <thead>
                        <tr className="border-b bg-gray-100 text-left">
                            <th className="px-6 py-3 text-sm font-medium text-gray-600">Order ID</th>
                            <th className="px-6 py-3 text-sm font-medium text-gray-600">Platform</th>
                            <th className="px-6 py-3 text-sm font-medium text-gray-600">Status</th>
                            <th className="px-6 py-3 text-sm font-medium text-gray-600">Order Date</th>
                            <th className="px-6 py-3 text-sm font-medium text-gray-600">Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {filteredOrders.length > 0 ? (
                            filteredOrders.map((order) => (
                                <tr key={order.orderId} className="border-b">
                                    <td className="px-6 py-3 text-sm text-gray-800">{order.orderId}</td>
                                    <td className="px-6 py-3 text-sm text-gray-800">{order.platform}</td>
                                    <td className="px-6 py-3 text-sm">
                                        <span
                                            className={`font-medium ${order.status === "In Progress"
                                                ? "text-blue-600"
                                                : order.status === "Completed"
                                                    ? "text-green-600"
                                                    : order.status === "In Transit"
                                                        ? "text-orange-600"
                                                        : "text-red-600"
                                                }`}
                                        >
                                            {order.status}
                                        </span>
                                    </td>
                                    <td className="px-6 py-3 text-sm text-gray-800">{order.orderDate}</td>
                                    <td className="px-6 py-3 text-sm">
                                        <EditOutlined
                                            onClick={() => showModal(order)}
                                            className="text-blue-600 cursor-pointer"
                                        />
                                    </td>
                                </tr>
                            ))
                        ) : (
                            <tr>
                                <td colSpan={5} className="px-6 py-3 text-sm text-center text-gray-500">
                                    No orders found.
                                </td>
                            </tr>
                        )}
                    </tbody>
                </table>
            </div>

            {/* Modal for editing order status */}
            <Modal
                title="Edit Order"
                open={isModalVisible}
                onOk={handleModalSave}
                onCancel={handleModalCancel}
                okText="Save"
                cancelText="Cancel"
                footer={null}  // Remove the default footer with Save button
                width={800}
                style={{ padding: "20px", borderRadius: "10px" }}
                bodyStyle={{ padding: "20px", backgroundColor: "#f9f9f9" }}
            >
                {selectedOrder && (
                    <div className="space-y-4">
                        <Row gutter={16}>
                            <Col span={12}>
                                <p><strong>Order ID:</strong> {selectedOrder.orderId}</p>
                            </Col>
                            <Col span={12}>
                                <p><strong>Platform:</strong> {selectedOrder.platform}</p>
                            </Col>
                        </Row>
                        <Row gutter={16}>
                            <Col span={12}>
                                <p><strong>Order Date:</strong> {selectedOrder.orderDate}</p>
                            </Col>
                            <Col span={12}>
                                <p><strong>Notes:</strong> {selectedOrder.notes}</p>
                            </Col>
                        </Row>

                        {/* Products Table */}
                        <h4 className="text-lg font-medium text-gray-600">Products</h4>
                        <Table
                            dataSource={selectedOrder.products}
                            columns={[
                                { title: 'Product ID', dataIndex: 'productId', key: 'productId' },
                                { title: 'Product Name', dataIndex: 'name', key: 'name' },
                                { title: 'Quantity', dataIndex: 'quantity', key: 'quantity' },
                                { title: 'Price', dataIndex: 'price', key: 'price' },
                                {
                                    title: 'Total Price',
                                    key: 'totalPrice',
                                    render: (_text, record: { quantity: number; price: number }) => record.quantity * record.price
                                },
                            ]}
                            pagination={false}
                            rowKey="productId"
                            summary={() => (
                                <Table.Summary.Row>
                                    <Table.Summary.Cell index={0} colSpan={4} align="right">Total Price</Table.Summary.Cell>
                                    <Table.Summary.Cell index={1}>
                                        ${calculateTotalPrice(selectedOrder.products)}
                                    </Table.Summary.Cell>
                                </Table.Summary.Row>
                            )}
                        />

                        {/* Status update with dropdown */}
                        <div>
                            <label className="block text-gray-600">Status</label>
                            <Select
                                defaultValue={selectedOrder.status}
                                onChange={(value) => setSelectedOrder({ ...selectedOrder, status: value })}
                                style={{ width: "100%" }}
                            >
                                <Option value="In Progress">In Progress</Option>
                                <Option value="Completed">Completed</Option>
                                <Option value="In Transit">In Transit</Option>
                                <Option value="Canceled">Canceled</Option>
                            </Select>
                        </div>
                    </div>
                )}
                {/* Custom Footer */}
                <div className="mt-4 flex justify-end">
                    <button
                        type="button"
                        onClick={handleModalSave}
                        className="ant-btn ant-btn-primary px-6 py-2 rounded-md"
                        style={{
                            backgroundColor: "#1890ff", // Ensure visibility with a solid color
                            borderColor: "#1890ff", // Make border color same as background
                            color: "white", // Ensure visibility with white text color
                        }}
                    >
                        Save
                    </button>
                    <button
                        type="button"
                        onClick={handleModalCancel}
                        className="ant-btn ant-btn-default px-6 py-2 rounded-md"
                    >
                        Cancel
                    </button>
                </div>
            </Modal>
        </div>
    );
};

export default OrderHistory;
