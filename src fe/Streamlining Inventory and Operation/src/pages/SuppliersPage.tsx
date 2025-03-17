import React, { useState, useEffect } from "react";
import { DeleteOutlined, EditOutlined, PlusOutlined } from "@ant-design/icons";
import { Button, Modal, Table, message, Form, Input, DatePicker } from "antd";
import dayjs from "dayjs";
import {
    getListSuppliers,
    getSupplierById,
    createSupplier,
    updateSupplierById,
    deleteSupplierById,
    checkSupplierExists,
} from "../fetch/supplier";

const SuppliersPage: React.FC = () => {
    const [suppliers, setSuppliers] = useState<any[]>([]);
    const [isModalVisible, setIsModalVisible] = useState(false);
    const [selectedSupplier, setSelectedSupplier] = useState<any>(null);
    const [form] = Form.useForm();

    const fetchSuppliers = async () => {
        try {
            const data = await getListSuppliers();
            setSuppliers(data.result.suppliers || []);
        } catch (error) {
            message.error("Failed to load suppliers");
        }
    };

    useEffect(() => {
        fetchSuppliers();
    }, []);

    const showModal = async (supplier?: any) => {
        if (supplier) {
            try {
                const supplierData = await getSupplierById(supplier.id);
                setSelectedSupplier(supplier);
                form.setFieldsValue({
                    ...supplierData,
                    createdAt: supplierData.createdAt ? dayjs(supplierData.createdAt) : null,
                });
            } catch (error) {
                message.error("Failed to fetch supplier details");
                return;
            }
        } else {
            setSelectedSupplier(null);
            form.resetFields();
        }
        setIsModalVisible(true);
    };

    const handleSave = async () => {
        try {
            // Validate form fields
            const supplierDetails = await form.validateFields();

            // Kiểm tra ngày tạo không vượt quá ngày hiện tại
            const today = dayjs().endOf("day");
            if (!supplierDetails.createdAt || dayjs(supplierDetails.createdAt).isAfter(today)) {
                message.error("Creation date cannot be in the future.");
                return;
            }

            supplierDetails.createdAt = supplierDetails.createdAt.format("YYYY-MM-DD");

            // Kiểm tra trùng tên, email, phone
            const isExist = await checkSupplierExists(supplierDetails);
            if (isExist) {
                message.error("Supplier with the same name, email, or phone already exists.");
                return;
            }

            if (selectedSupplier) {
                await updateSupplierById(selectedSupplier.id, supplierDetails);
                message.success("Supplier updated successfully!");
            } else {
                await createSupplier(supplierDetails);
                message.success("Supplier added successfully!");
            }

            setIsModalVisible(false);
            fetchSuppliers();
        } catch (error: any) {
            console.error("Save error:", error);
            message.error(error.message || "Failed to save supplier");
        }
    };

    const handleDelete = async (id: number) => {
        try {
            await deleteSupplierById(id);
            message.success("Supplier deleted successfully!");
            setSuppliers((prev) => prev.filter((supplier) => supplier.id !== id));
        } catch (error: any) {
            console.error("Delete error:", error);
            message.error(error.message || "Failed to delete supplier");
        }
    };

    return (
        <div className="p-6 bg-gray-50">
            <h3 className="text-xl font-semibold text-gray-900">Manage Suppliers</h3>
            <Button type="primary" icon={<PlusOutlined />} onClick={() => showModal()} className="mb-6 mt-6 bg-blue-600 text-white">
                Add Supplier
            </Button>
            <Table dataSource={suppliers} rowKey="id" bordered>
                <Table.Column title="Name" dataIndex="name" key="name" align="center" />
                <Table.Column title="Email" dataIndex="email" key="email" align="center" />
                <Table.Column title="Phone" dataIndex="phone" key="phone" align="center" />
                <Table.Column title="Created At" dataIndex="createdAt" key="createdAt" align="center" render={(text) => text ? dayjs(text).format("YYYY-MM-DD") : "-"} />
                <Table.Column title="Actions" key="actions" align="center" render={(_, record: any) => (
                    <div className="flex justify-center gap-2">
                        <Button icon={<EditOutlined />} onClick={() => showModal(record)} />
                        <Button icon={<DeleteOutlined />} danger onClick={() => handleDelete(record.id)} />
                    </div>
                )} />
            </Table>
            <Modal
                title={selectedSupplier ? "Edit Supplier" : "Add Supplier"}
                open={isModalVisible}
                onOk={handleSave}
                onCancel={() => setIsModalVisible(false)}
                okText="Save"
                cancelText="Cancel"
                footer={[
                    <Button key="cancel" onClick={() => setIsModalVisible(false)}>
                        Cancel
                    </Button>,
                    <Button key="save" type="primary" onClick={handleSave} className="bg-blue-600 text-white">
                        Save
                    </Button>,
                ]}
            >
                <Form form={form} layout="vertical">
                    <Form.Item 
                        name="name" 
                        label="Supplier Name"
                        rules={[
                            { required: true, message: "Please enter a supplier name" },
                            { pattern: /^[A-Za-z\s]+$/, message: "Name must only contain letters and spaces" },
                            { min: 3, max: 50, message: "Name must be between 3 and 50 characters" }
                        ]}
                    >
                        <Input placeholder="Supplier Name" />
                    </Form.Item>

                    <Form.Item 
                        name="email" 
                        label="Email"
                        rules={[
                            { required: true, message: "Please enter an email" },
                            { type: "email", message: "Invalid email format (e.g., example@email.com)" }
                        ]}
                    >
                        <Input placeholder="Email" />
                    </Form.Item>

                    <Form.Item 
                        name="phone" 
                        label="Phone Number" 
                        rules={[
                            { required: true, message: "Please enter a phone number" },
                            { pattern: /^0[0-9]{9,10}$/, message: "Phone number must be a valid Vietnamese number (10-11 digits)" }
                        ]}
                    >
                        <Input placeholder="Phone Number" />
                    </Form.Item>

                    <Form.Item 
                        name="createdAt" 
                        label="Created At"
                        rules={[
                            { required: true, message: "Please select a creation date" }
                        ]}
                    >
                        <DatePicker format="YYYY-MM-DD" className="w-full" allowClear />
                    </Form.Item>
                </Form>
            </Modal>
        </div>
    );
};

export default SuppliersPage;
