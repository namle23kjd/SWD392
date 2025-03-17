import React, { useState, useEffect } from "react";
import { Table, Modal, Input, Button } from "antd";
import { EditOutlined, DeleteOutlined, FileAddOutlined } from "@ant-design/icons";
import { toast } from "react-toastify";
import { getAllPlatforms, createPlatform, updatePlatform, deletePlatform, Platform } from "../fetch/platform";

const PlatformsPage: React.FC = () => {
    const [platforms, setPlatforms] = useState<Platform[]>([]);
    const [isModalVisible, setIsModalVisible] = useState(false);
    const [selectedPlatform, setSelectedPlatform] = useState<Platform | null>(null);
    const [platformDetails, setPlatformDetails] = useState({
        name: "",
        isActive: true,
    });

    // Fetch all platforms on component mount
    useEffect(() => {
        const fetchPlatforms = async () => {
            try {
                const data = await getAllPlatforms();
                setPlatforms(data);
            } catch (error) {
                toast.error("Failed to fetch platforms.");
            }
        };
        fetchPlatforms();
    }, []);

    const showModal = (platform?: Platform) => {
        setSelectedPlatform(platform || null);
        setPlatformDetails(platform || { name: "", isActive: true });
        setIsModalVisible(true);
    };

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setPlatformDetails((prev) => ({ ...prev, [name]: value }));
    };

    const handleSave = async () => {
        if (!platformDetails.name) {
            toast.error("Please fill in the platform name.");
            return;
        }

        try {
            if (selectedPlatform) {
                // Update platform
                await updatePlatform(selectedPlatform.id, platformDetails);
                setPlatforms((prev) =>
                    prev.map((p) => (p.id === selectedPlatform.id ? { ...p, ...platformDetails } : p))
                );
                toast.success("Platform updated successfully!");
            } else {
                // Add new platform
                const newPlatform = await createPlatform(platformDetails);
                setPlatforms([...platforms, newPlatform]);
                toast.success("Platform added successfully!");
            }
        } catch (error) {
            toast.error("Error saving platform!");
        }

        setIsModalVisible(false);
        setPlatformDetails({ name: "", isActive: true });
    };

    const handleDelete = async (id: string) => {
        try {
            await deletePlatform(id);
            setPlatforms(platforms.filter((p) => p.id !== id));
            toast.success("Platform deleted successfully!");
        } catch (error) {
            toast.error("Error deleting platform!");
        }
    };

    return (
        <div className="p-6 bg-gray-50">
            <h3 className="text-xl font-semibold text-gray-900">Manage Platforms</h3>

            {/* Add Platform Button */}
            <Button
                type="primary"
                icon={<FileAddOutlined />}
                onClick={() => showModal()}
                className="mb-4 mt-6 bg-blue-600 text-white hover:bg-blue-700"
            >
                Add Platform
            </Button>

            {/* Platforms Table */}
            <Table dataSource={platforms} rowKey="id">
                <Table.Column title="Name" dataIndex="name" key="name" />
                <Table.Column
                    title="Active Status"
                    dataIndex="isActive"
                    key="isActive"
                    render={(text) => (text ? <span className="text-green-600">Active</span> : <span className="text-red-600">Inactive</span>)}
                />
                <Table.Column
                    title="Actions"
                    key="actions"
                    render={(_, record: any) => (
                        <div className="flex gap-2">
                            <Button icon={<EditOutlined />} onClick={() => showModal(record)} />
                            <Button icon={<DeleteOutlined />} danger onClick={() => handleDelete(record.id)} />
                        </div>
                    )}
                />
            </Table>

            {/* Modal for Add/Edit Platform */}
            <Modal
                title={selectedPlatform ? "Edit Platform" : "Add Platform"}
                visible={isModalVisible}
                onOk={handleSave}
                onCancel={() => setIsModalVisible(false)}
                okText="Save"
                cancelText="Cancel"
                okButtonProps={{
                    className: "bg-blue-600 text-white hover:bg-blue-700", // Save button styling
                }}
                cancelButtonProps={{
                    className: "border-gray-300 text-gray-600 hover:bg-gray-100", // Cancel button styling
                }}
            >
                <Input
                    name="name"
                    placeholder="Platform Name"
                    value={platformDetails.name}
                    onChange={handleInputChange}
                    className="mb-2"
                />
                <div>
                    <label className="block text-gray-600">Status</label>
                    <select
                        name="isActive"
                        value={platformDetails.isActive ? "active" : "inactive"}
                        onChange={(e) => setPlatformDetails({ ...platformDetails, isActive: e.target.value === "active" })}
                        className="w-full border rounded-lg px-4 py-2 mt-1"
                    >
                        <option value="active">Active</option>
                        <option value="inactive">Inactive</option>
                    </select>
                </div>
            </Modal>
        </div>
    );
};

export default PlatformsPage;
