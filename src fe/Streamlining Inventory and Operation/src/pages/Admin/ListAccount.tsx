import React, { useState } from "react";
import Breadcrumb from '../../components/Breadcrumbs/Breadcrumb';
import { Link, useNavigate } from "react-router-dom";
import { toast } from "react-toastify"; // Import toast for error messages
import { EditOutlined, DeleteOutlined } from '@ant-design/icons'; // Import icons from Ant Design
import { Modal } from 'antd'; // Import Modal for confirmation dialog

// Sample data for the accounts (replace with actual data in a real scenario)
const sampleAccounts = [
    { id: "U001", username: "JohnDoe", email: "johndoe@gmail.com", phone: "123-456-7890", role: "Admin", status: "Active" },
    { id: "U002", username: "JaneSmith", email: "janesmith@gmail.com", phone: "987-654-3210", role: "User", status: "Inactive" },
    { id: "U003", username: "MikeJohnson", email: "mikejohnson@gmail.com", phone: "555-123-4567", role: "User", status: "Active" },
];

const ListAccount: React.FC = () => {
    const [accounts, setAccounts] = useState(sampleAccounts);
    const [searchTerm, setSearchTerm] = useState("");
    const [isDeleting, setIsDeleting] = useState<string | null>(null); // State to track the account being deleted
    const navigate = useNavigate();

    // Function to filter accounts based on search term
    const handleSearch = (e: React.ChangeEvent<HTMLInputElement>) => {
        setSearchTerm(e.target.value);
    };

    // Filter accounts by search term
    const filteredAccounts = accounts.filter((account) =>
        account.username.toLowerCase().includes(searchTerm.toLowerCase()) ||
        account.email.toLowerCase().includes(searchTerm.toLowerCase()) ||
        account.status.toLowerCase().includes(searchTerm.toLowerCase())
    );

    // Function to handle the Export Data button
    const handleExport = () => {
        // Example export functionality, you can export it as a CSV or JSON
        const data = JSON.stringify(filteredAccounts, null, 2);
        const blob = new Blob([data], { type: "application/json" });
        const link = document.createElement("a");
        link.href = URL.createObjectURL(blob);
        link.download = "accounts_data.csv";
        link.click();
        toast.success("Data exported successfully!");
    };

    // Function to handle the edit action
    const handleEdit = (id: string) => {
        // Implement edit functionality (e.g., navigate to edit page)
        navigate(`/admin/edit-account/${id}`);
    };

    // Function to show delete confirmation modal
    const handleDelete = (id: string) => {
        setIsDeleting(id); // Set the account id to be deleted
        Modal.confirm({
            title: "Are you sure you want to delete this account?",
            content: "This action cannot be undone.",
            okText: "Yes",
            okType: "danger",
            cancelText: "No",
            onOk: () => deleteAccount(id), // Delete account on confirmation
            onCancel: () => setIsDeleting(null), // Reset deleting state if canceled
        });
    };

    // Function to delete the account
    const deleteAccount = (id: string) => {
        setAccounts(accounts.filter(account => account.id !== id)); // Remove the account from the list
        setIsDeleting(null); // Reset deleting state
        toast.success("Account deleted successfully!"); // Show success message
    };

    return (
        <>
            <Breadcrumb pageName="Manage Account" />
            <div className="grid-cols-1 gap-9 sm:grid-cols-2">
                <div className="flex flex-col gap-9">
                    {/* Account Management */}
                    <div className="rounded-sm border border-stroke bg-white shadow-default dark:border-strokedark dark:bg-boxdark">
                        <div className="border-b border-stroke py-4 px-6.5 dark:border-strokedark">
                            <h3 className="font-medium text-black dark:text-white">Manage Accounts</h3>
                        </div>
                        <div className="flex flex-col gap-5.5 p-6.5">
                            {/* Search Bar */}
                            <div className="flex items-center gap-4 mb-4">
                                <input
                                    type="text"
                                    placeholder="Search by username, email, or status"
                                    className="w-full rounded-lg border-[1.5px] border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary dark:border-form-strokedark dark:bg-form-input dark:focus:border-primary"
                                    value={searchTerm}
                                    onChange={handleSearch}
                                />
                                <button
                                    onClick={handleExport}
                                    className="min-w-40 bg-blue-600 text-white py-2 px-4 rounded-md"
                                >
                                    Export Data
                                </button>

                                <Link to="/admin/add-account"
                                    className="min-w-40 bg-green-600 text-white py-2 px-4 rounded-md"
                                >
                                    Create account +
                                </Link>
                            </div>

                            {/* Account Table */}
                            <table className="w-full">
                                <thead>
                                    <tr>
                                        <th className="px-4 py-2">Account ID</th>
                                        <th className="px-4 py-2">Username</th>
                                        <th className="px-4 py-2">Email</th>
                                        <th className="px-4 py-2">Phone Number</th>
                                        <th className="px-4 py-2">Role</th>
                                        <th className="px-4 py-2">Status</th>
                                        <th className="px-4 py-2">Action</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {filteredAccounts.length > 0 ? (
                                        filteredAccounts.map((account) => (
                                            <tr key={account.id}>
                                                <td className="px-4 py-2 text-center">{account.id}</td>
                                                <td className="px-4 py-2 text-center">{account.username}</td>
                                                <td className="px-4 py-2 text-center">{account.email}</td>
                                                <td className="px-4 py-2 text-center">{account.phone}</td>
                                                <td className="px-4 py-2 text-center">{account.role}</td>
                                                <td className={`px-4 py-2 text-center text-${account.status === 'Active' ? 'green-600' : 'red-600'}`}>
                                                    {account.status}
                                                </td>
                                                <td className="px-4 py-2 text-center">
                                                    <button
                                                        onClick={() => handleEdit(account.id)}
                                                        className="text-blue-600 hover:text-blue-800 mr-4"
                                                    >
                                                        <EditOutlined />
                                                    </button>
                                                    <button
                                                        onClick={() => handleDelete(account.id)}
                                                        className="text-red-600 hover:text-red-800"
                                                    >
                                                        <DeleteOutlined />
                                                    </button>
                                                </td>
                                            </tr>
                                        ))
                                    ) : (
                                        <tr>
                                            <td colSpan={7} className="text-center py-2">
                                                No accounts found.
                                            </td>
                                        </tr>
                                    )}
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </>
    );
};

export default ListAccount;
