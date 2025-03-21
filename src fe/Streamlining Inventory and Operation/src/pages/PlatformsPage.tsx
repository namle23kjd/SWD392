import React, { useState, useEffect } from "react";
import { Table, Modal, Input, Button, Spin } from "antd";
import { EditOutlined, DeleteOutlined, FileAddOutlined } from "@ant-design/icons";
import { toast } from "react-toastify";
import { getAllPlatforms, createPlatform, updatePlatform, deletePlatform } from "../fetch/platform";
import { useQuery } from "@tanstack/react-query";
import { queryClient } from "../util/queryClient";

interface Platform {
    platformId: string;
    name: string;
    isActive: boolean;
    apiKey: string;
}

const PlatformsPage: React.FC = () => {
    const [platforms, setPlatforms] = useState([]);
    const [isModalVisible, setIsModalVisible] = useState(false);  // Add/Update Platform modal state
    const [deleteModalVisible, setDeleteModalVisible] = useState(false);  // Delete Platform modal state
    const [selectedPlatform, setSelectedPlatform] = useState<Platform | null>(null);
    const [platformDetails, setPlatformDetails] = useState({
        name: "",
        isActive: true,
        apiKey: "",
    });

    // State to store errors
    const [errors, setErrors] = useState({
        name: "",
        apiKey: "",
    });

    const { data, isPending } = useQuery({
        queryKey: ["platforms"],
        queryFn: getAllPlatforms,
    });

    const fetchPlatforms = async () => {
        if (data) {
            setPlatforms(data.result);
        }
    };

    useEffect(() => {
        fetchPlatforms();
    }, [data]);

    const showModal = (platform?: any) => {
        setSelectedPlatform(platform || null);
        setPlatformDetails(platform || { name: "", isActive: true, apiKey: "" });
        setIsModalVisible(true);
    };

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setPlatformDetails((prev) => ({ ...prev, [name]: value }));
    };

    // Validate form fields
    const validateForm = () => {
        const errors: any = {};
        if (!platformDetails.name) errors.name = "Platform name is required!";
        if (!platformDetails.apiKey) errors.apiKey = "API key is required!";
        setErrors(errors);
        return Object.keys(errors).length === 0;
    };

    const handleSave = async () => {
        if (!validateForm()) {
            return;
        }

        try {
            if (selectedPlatform) {
                // Update platform
                const response = await updatePlatform(selectedPlatform.platformId, platformDetails);
                if (response.statusCode === 200) {
                    queryClient.invalidateQueries({ queryKey: ["platforms"] });
                    toast.success("Platform updated successfully!");
                } else {
                    toast.error(response[0]);
                }
            } else {
                const response = await createPlatform(platformDetails);
                if (response.statusCode === 201) {
                    queryClient.invalidateQueries({ queryKey: ["platforms"] });
                    toast.success("Platform added successfully!");
                } else {
                    toast.error(response[0]);
                }
            }
        } catch (error) {
            toast.error("Error saving platform!");
        }

        setIsModalVisible(false);
        setPlatformDetails({ name: "", isActive: true, apiKey: "" });
        setErrors({ name: "", apiKey: "" });
    };

    // Handle delete confirmation modal
    const handleDelete = (platform: Platform) => {
        setSelectedPlatform(platform);
        setDeleteModalVisible(true); // Open delete confirmation modal
    };

    const handleDeleteConfirm = async () => {
        try {
            await deletePlatform(selectedPlatform?.platformId || "");
            queryClient.invalidateQueries({ queryKey: ["platforms"] });
            toast.success("Platform deleted successfully!");
        } catch (error) {
            toast.error("Error deleting platform!");
        }

        setDeleteModalVisible(false); // Close the delete confirmation modal
    };

    const handleCloseModal = () => {
        setErrors({ name: "", apiKey: "" });
        setIsModalVisible(false);
    };

    const handleCloseDeleteModal = () => {
        setDeleteModalVisible(false); // Close delete confirmation modal
    };

    return (
        <div className="p-6 bg-gray-50">
            <h3 className="text-xl font-semibold text-gray-900">Manage Platforms</h3>
            <Button
                type="primary"
                icon={<FileAddOutlined />}
                onClick={() => showModal()}
                className="mb-4 mt-6 bg-blue-600 text-white hover:bg-blue-700"
            >
                Add Platform
            </Button>
            {isPending ? <Spin /> : (
                <Table
                    dataSource={platforms}
                    rowKey="platformId"
                    pagination={{
                        pageSize: 8, // Set pagination to 8 items per page
                    }}
                >
                    <Table.Column title="Name" dataIndex="name" key="name" />
                    <Table.Column
                        title="API Key"
                        dataIndex="apiKey"
                        key="apiKey"
                    />
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
                                <Button icon={<DeleteOutlined />} danger onClick={() => handleDelete(record)} />
                            </div>
                        )}
                    />
                </Table>
            )}
            {/* Add/Edit Platform Modal */}
            <Modal
                title={selectedPlatform ? "Edit Platform" : "Add Platform"}
                open={isModalVisible}
                onOk={handleSave}
                onCancel={handleCloseModal}
                okText="Save"
                cancelText="Cancel"
                footer={[
                    <Button key="cancel" onClick={handleCloseModal}>
                        Cancel
                    </Button>,
                    <Button key="save" type="primary" onClick={handleSave} className="bg-blue-600 text-white">
                        Save
                    </Button>,
                ]}
            >
                <Input
                    name="name"
                    placeholder="Platform Name"
                    value={platformDetails.name}
                    onChange={handleInputChange}
                    className="mb-2"
                />
                {errors.name && <span className="text-red-500">{errors.name}</span>}  {/* Error message for name */}

                <div>
                    <label className="block text-gray-600">API Key</label>
                    <Input
                        name="apiKey"
                        placeholder="API Key"
                        value={platformDetails.apiKey}
                        onChange={handleInputChange}
                        className="mb-2"
                    />
                    {errors.apiKey && <span className="text-red-500">{errors.apiKey}</span>}  {/* Error message for apiKey */}
                </div>

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

            {/* Delete Confirmation Modal */}
            <Modal
                title="Delete Platform"
                open={deleteModalVisible}
                onOk={handleDeleteConfirm}
                onCancel={handleCloseDeleteModal}
                okText="Delete"
                cancelText="Cancel"
                okButtonProps={{
                    danger: true
                }}
            >
                <p>Are you sure you want to delete this platform?</p>
            </Modal>
        </div>
    );
};

export default PlatformsPage;
