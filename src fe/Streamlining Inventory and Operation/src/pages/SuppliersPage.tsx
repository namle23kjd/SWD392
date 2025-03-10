import React, { useState, useEffect } from "react";
import { toast } from "react-toastify"; // Hiển thị thông báo
import { Table, Modal, Input, Button } from "antd";
import { DeleteOutlined, EditOutlined, PlusOutlined } from "@ant-design/icons";

const SuppliersPage: React.FC = () => {
    // State storage Supplier
    const [suppliers, setSuppliers] = useState<any[]>([]);
    const [isModalVisible, setIsModalVisible] = useState(false);
    const [selectedSupplier, setSelectedSupplier] = useState<any>(null);
    const [supplierDetails, setSupplierDetails] = useState({
        name: "",
        email: "",
        phone: "",
        createAt: new Date().toISOString().split("T")[0], // date
    });

    // Fetch API
    useEffect(() => {
        const fetchSuppliers = async () => {
            const fetchedSuppliers = [
                { id: "S001", name: "Supplier A", email: "a@example.com", phone: "123 456 7890", createAt: "2024-03-01" },
                { id: "S002", name: "Supplier B", email: "b@example.com", phone: "098 765 4321", createAt: "2024-03-05" },
            ];
            setSuppliers(fetchedSuppliers);
        };
        fetchSuppliers();
    }, []);

    const showModal = (supplier?: any) => {
        setSelectedSupplier(supplier || null);
        setSupplierDetails(supplier || {
            name: "",
            email: "",
            phone: "",
            createAt: new Date().toISOString().split("T")[0], // date
        });
        setIsModalVisible(true);
    };

    
    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setSupplierDetails((prev) => ({ ...prev, [name]: value }));
    };

    // Storage data when Submit (adđ / edit)
    const handleSave = () => {
        if (!supplierDetails.name || !supplierDetails.email || !supplierDetails.phone) {
            toast.error("Please fill in all fields.");
            return;
        }

        if (selectedSupplier) {
            // Cập nhật Supplier
            setSuppliers((prev) =>
                prev.map((s) => (s.id === selectedSupplier.id ? { ...s, ...supplierDetails } : s))
            );
            toast.success("Supplier updated successfully!");
        } else {
            // Thêm mới Supplier
            const newSupplier = { id: Date.now().toString(), ...supplierDetails };
            setSuppliers([...suppliers, newSupplier]);
            toast.success("Supplier added successfully!");
        }

        setIsModalVisible(false);
        setSupplierDetails({ name: "", email: "", phone: "", createAt: new Date().toISOString().split("T")[0] });
    };

    // Delete Supplier
    const handleDelete = (id: string) => {
        setSuppliers(suppliers.filter((s) => s.id !== id));
        toast.error("Supplier deleted!");
    };

    return (
        <div className="p-6 bg-gray-50">
            <h3 className="text-xl font-semibold text-gray-900">Manage Suppliers</h3>

            {/* add  Supplier */}
            <Button
                type="primary"
                icon={<PlusOutlined />}
                onClick={() => showModal()}
                className="mb-4 mt-6 bg-blue-600 text-white"
            >
                Add Supplier
            </Button>

            {/* View Supplier */}
            <Table dataSource={suppliers} rowKey="id" bordered>
                <Table.Column 
                    title={<div className="bg-blue-600 text-white p-2 text-center">Name</div>} 
                    dataIndex="name" 
                    key="name" 
                    align="center"
                />
                <Table.Column 
                    title={<div className="bg-blue-600 text-white p-2 text-center">Email</div>} 
                    dataIndex="email" 
                    key="email" 
                    align="center"
                />
                <Table.Column 
                    title={<div className="bg-blue-600 text-white p-2 text-center">Phone</div>} 
                    dataIndex="phone" 
                    key="phone" 
                    align="center"
                />
                <Table.Column 
                    title={<div className="bg-blue-600 text-white p-2 text-center">Created At</div>} 
                    dataIndex="createAt" 
                    key="createAt" 
                    align="center"
                />
                <Table.Column
                    title={<div className="bg-blue-600 text-white p-2 text-center">Actions</div>}
                    key="actions"
                    align="center"
                    render={(_, record: any) => (
                        <div className="flex justify-center gap-2">
                            <Button icon={<EditOutlined />} onClick={() => showModal(record)} />
                            <Button icon={<DeleteOutlined />} danger onClick={() => handleDelete(record.id)} />
                        </div>
                    )}
                />
            </Table>

            {/* Modal add/edit Supplier */}
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
                <Input
                    name="name"
                    placeholder="Supplier Name"
                    value={supplierDetails.name}
                    onChange={handleInputChange}
                    className="mb-2"
                />
                <Input
                    name="email"
                    type="email"
                    placeholder="Email"
                    value={supplierDetails.email}
                    onChange={handleInputChange}
                    className="mb-2"
                />
                <Input
                    name="phone"
                    placeholder="Phone Number"
                    value={supplierDetails.phone}
                    onChange={handleInputChange}
                    className="mb-2"
                />
                <Input
                    name="createAt"
                    type="date"
                    value={supplierDetails.createAt}
                    onChange={handleInputChange}
                    className="mb-2"
                />
            </Modal>
        </div>
    );
};

export default SuppliersPage;
