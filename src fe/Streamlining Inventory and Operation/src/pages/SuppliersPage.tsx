import { DeleteOutlined, EditOutlined, PlusOutlined } from "@ant-design/icons";
import { keepPreviousData, useQuery } from "@tanstack/react-query";
import { Button, Spin, Table, message, Modal, Input } from "antd";
import React, { useEffect, useState } from "react";
import { toast } from "react-toastify";
import {
    createSupplier,
    deleteSupplierById,
    getListSuppliers,
    updateSupplierById,
} from "../fetch/supplier";
import { formatDate } from "../util/convertUtils";
import { queryClient } from "../util/queryClient";

const SuppliersPage: React.FC = () => {
    const [suppliers, setSuppliers] = useState<any[]>([]);
    const [isModalVisible, setIsModalVisible] = useState(false);
    const [isDeleteModalVisible, setIsDeleteModalVisible] = useState(false);
    const [selectedSupplier, setSelectedSupplier] = useState<any>(null);
    const [supplierToDelete, setSupplierToDelete] = useState<number | null>(null);

    // State for input fields
    const [name, setName] = useState<string>('');
    const [email, setEmail] = useState<string>('');
    const [phone, setPhone] = useState<string>('');
    const [errors, setErrors] = useState<any>({});

    const [pageNumber, setPageNumber] = useState(1);
    const pageSize = 7;

    const { data, isLoading } = useQuery({
        queryKey: ["suppliers", pageNumber, pageSize],
        queryFn: () => getListSuppliers(pageNumber, pageSize),
        placeholderData: keepPreviousData
    });

    const fetchSuppliers = async () => {
        try {
            if (data) {
                setSuppliers(data.result.suppliers || []);
            }
        } catch (error) {
            message.error("Failed to load suppliers");
        }
    };

    useEffect(() => {
        fetchSuppliers();
    }, [data]);

    const showModal = async (supplier?: any) => {
        if (supplier) {
            // Tìm nhà cung cấp trong mảng suppliers thay vì gọi API
            const supplierData = suppliers.find(item => item.supplierId === supplier.supplierId);
            if (supplierData) {
                // Cập nhật giá trị vào form trực tiếp
                setName(supplierData.name);
                setEmail(supplierData.email);
                setPhone(supplierData.phone);
                setSelectedSupplier(supplierData);
            }
        } else {
            // Reset form khi mở modal thêm mới
            setSelectedSupplier(null);
            setName('');
            setEmail('');
            setPhone('');
            setErrors({});
        }
        setIsModalVisible(true);
    };

    const validateForm = () => {
        const newErrors: any = {};

        // Validate name
        if (!name) newErrors.name = "Please enter a supplier name";

        // Validate email with regex
        const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
        if (!email) newErrors.email = "Please enter an email";
        else if (!emailRegex.test(email)) newErrors.email = "Invalid email format";

        // Validate phone number with regex
        const phoneRegex = /^0[0-9]{3}-[0-9]{3}-[0-9]{3}$/;
        if (!phone) newErrors.phone = "Please enter a phone number";
        else if (!phoneRegex.test(phone)) newErrors.phone = "Phone number must be in the format XXXX-XXX-XXX";

        setErrors(newErrors);
        return Object.keys(newErrors).length === 0;
    };

    const handleSave = async () => {
        if (!validateForm()) return;
        const supplierDetails = { name, email, phone };

        try {
            if (selectedSupplier) {
                const response = await updateSupplierById(selectedSupplier.supplierId, supplierDetails);
                if (response.statusCode === 200) {
                    toast.success("Supplier updated successfully!");
                    setIsModalVisible(false);
                    // Reset form sau khi lưu
                    setName('');
                    setEmail('');
                    setPhone('');
                    queryClient.invalidateQueries({ queryKey: ['suppliers'] });
                } else {
                    toast.error(response[0]);
                }
            } else {
                const response = await createSupplier(supplierDetails);
                if (response.statusCode === 201) {
                    toast.success("Supplier added successfully!");
                    setIsModalVisible(false);
                    // Reset form sau khi lưu
                    setName('');
                    setEmail('');
                    setPhone('');
                    queryClient.invalidateQueries({ queryKey: ['suppliers'] });
                } else {
                    toast.error(response[0]);
                }
            }
            fetchSuppliers(); // Cập nhật lại danh sách nhà cung cấp
        } catch (error: any) {
            console.error("Save error:", error);
            message.error(error.message || "Failed to save supplier");
        }
    };

    const handleCancelModal = () => {
        setIsModalVisible(false);
        // Reset form khi đóng modal
        setName('');
        setEmail('');
        setPhone('');
        setErrors({});
    };

    const handleDelete = (id: number) => {
        setSupplierToDelete(id);
        setIsDeleteModalVisible(true);
    };

    const confirmDeleteSupplier = async () => {
        if (supplierToDelete) {
            const response = await deleteSupplierById(supplierToDelete);
            if (response.statusCode === 200) {
                toast.success("Supplier deleted successfully!");
                queryClient.invalidateQueries({ queryKey: ['suppliers'] });
                setSuppliers((prev) => prev.filter
                    ((supplier) => supplier.id !== supplierToDelete));
            } else {
                toast.error(response[0]);
            }
            setIsDeleteModalVisible(false);
        }
    };

    const handlePageChange = (newPage: number) => {
        setPageNumber(newPage);
    };

    // Automatically format phone number with dashes
    const formatPhoneNumber = (value: string) => {
        // Remove any non-numeric characters
        const numbers = value.replace(/\D/g, '');
        if (numbers.length <= 4) return numbers;
        if (numbers.length <= 7) return `${numbers.slice(0, 4)}-${numbers.slice(4)}`;
        return `${numbers.slice(0, 4)}-${numbers.slice(4, 7)}-${numbers.slice(7, 10)}`;
    };

    return (
        <div className="p-6 bg-gray-50">
            <h3 className="text-xl font-semibold text-gray-900">Manage Suppliers</h3>
            <Button type="primary" icon={<PlusOutlined />} onClick={() => showModal()} className="mb-6 mt-6 bg-blue-600 text-white">
                Add Supplier
            </Button>
            {isLoading ? <Spin /> : (
                <Table dataSource={suppliers} rowKey="id" bordered pagination={{
                    current: pageNumber,
                    pageSize: pageSize,
                    total: data?.result?.totalCount || 0,
                    onChange: handlePageChange,
                    showSizeChanger: false,
                }}>
                    <Table.Column title="Name" dataIndex="name" key="name" align="center" />
                    <Table.Column title="Email" dataIndex="email" key="email" align="center" />
                    <Table.Column title="Phone" dataIndex="phone" key="phone" align="center" />
                    <Table.Column title="Created At" dataIndex="createAt" key="createAt" align="center" render={(text) => formatDate(text)} />
                    <Table.Column title="Actions" key="actions" align="center" render={(_, record: any) => (
                        <div className="flex justify-center gap-2">
                            <Button icon={<EditOutlined />} onClick={() => showModal(record)} />
                            <Button icon={<DeleteOutlined />} danger onClick={() => handleDelete(record.supplierId)} />
                        </div>
                    )} />
                </Table>
            )}

            <Modal
                title={selectedSupplier ? "Edit Supplier" : "Add Supplier"}
                open={isModalVisible}
                okText="Save"
                cancelText="Cancel"
                onCancel={handleCancelModal}
                footer={[
                    <Button key="cancel" onClick={handleCancelModal}>
                        Cancel
                    </Button>,
                    <Button key="save" type="primary" onClick={handleSave} className="bg-blue-600 text-white">
                        Save
                    </Button>,
                ]}
            >
                <div>
                    <div>
                        <label>Supplier Name</label>
                        <Input
                            placeholder="Supplier Name"
                            value={name}
                            onChange={(e) => setName(e.target.value)}
                            style={{ marginBottom: "10px" }}
                        />
                        {errors.name && <div style={{ color: 'red' }}>{errors.name}</div>}
                    </div>
                    <div>
                        <label>Email</label>
                        <Input
                            placeholder="Email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            style={{ marginBottom: "10px" }}
                        />
                        {errors.email && <div style={{ color: 'red' }}>{errors.email}</div>}
                    </div>
                    <div>
                        <label>Phone Number</label>
                        <Input
                            placeholder="Phone Number"
                            value={formatPhoneNumber(phone)}
                            onChange={(e) => setPhone(formatPhoneNumber(e.target.value))}
                            style={{ marginBottom: "10px" }}
                        />
                        {errors.phone && <div style={{ color: 'red' }}>{errors.phone}</div>}
                    </div>
                </div>
            </Modal>

            <Modal
                onClose={() => setIsDeleteModalVisible(false)}
                onCancel={() => setIsDeleteModalVisible(false)}
                title="Confirm Deletion"
                open={isDeleteModalVisible}
                okText="Delete"
                cancelText="Cancel"
                centered
                footer={[
                    <Button key="cancel" onClick={() => setIsDeleteModalVisible(false)}>
                        Cancel
                    </Button>,
                    <Button key="save" type="primary" onClick={confirmDeleteSupplier} className="bg-red-500 text-white">
                        Delete
                    </Button>,
                ]}
            >
                <p>Are you sure you want to delete this supplier? This action cannot be undone.</p>
            </Modal>
        </div>
    );
};

export default SuppliersPage;
