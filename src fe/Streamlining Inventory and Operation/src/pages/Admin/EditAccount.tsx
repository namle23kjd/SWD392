import React, { useState, ChangeEvent, FormEvent, useEffect } from "react";
import Breadcrumb from '../../components/Breadcrumbs/Breadcrumb';
import { useNavigate, useParams } from "react-router-dom";
import { toast } from "react-toastify"; // For displaying error messages

// Account creation form validation rules
const validateAccountData = (accountData: any) => {
    const { username, email, phone, role, status } = accountData;

    if (!username || !email || !phone || !role || !status) {
        return "All fields are required.";
    }

    const emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;
    if (!emailRegex.test(email)) {
        return "Please enter a valid email address.";
    }

    const phoneRegex = /^\d{3}-\d{3}-\d{4}$/;
    if (!phoneRegex.test(phone)) {
        return "Please enter a valid phone number (format: XXX-XXX-XXXX).";
    }

    return null; // No errors
};

const EditAccount: React.FC = () => {
    const { id } = useParams(); // Extract account ID from the URL
    const navigate = useNavigate();
    const [accountDetails, setAccountDetails] = useState({
        username: "",
        email: "",
        phone: "",
        role: "",
        status: "",
    });

    useEffect(() => {
        // Fetch the account data based on the account ID (for now, simulating with a sample account)
        const fetchAccountData = async () => {
            // Simulate fetching account data (in a real app, you'd fetch this from an API)
            // Example:
            // const response = await axios.get(`/api/accounts/${id}`);
            const sampleAccount = { // Simulating a response from the server
                username: "JohnDoe",
                email: "johndoe@gmail.com",
                phone: "123-456-7890",
                role: "Admin",
                status: "Active",
            };
            setAccountDetails(sampleAccount);
        };

        fetchAccountData();
    }, [id]); // Re-fetch account data when the account ID changes

    // Handle input changes
    const handleInputChange = (e: ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
        const { name, value } = e.target;
        setAccountDetails((prev) => ({ ...prev, [name]: value }));
    };

    // Handle form submission
    const handleSubmit = async (e: FormEvent) => {
        e.preventDefault();

        // Validate the account data
        const validationError = validateAccountData(accountDetails);
        if (validationError) {
            toast.error(validationError); // Show error toast if validation fails
            return;
        }

        try {
            // Simulate API call to update the account details
            // await axios.put(`/api/accounts/${id}`, accountDetails);

            // Show success toast and navigate to account management page
            toast.success("Account updated successfully!");
            navigate("/admin/accounts"); // Redirect to the manage accounts page
        } catch (error) {
            toast.error("Failed to update account.");
        }
    };

    return (
        <>
            <Breadcrumb pageName="Edit Account" />

            <div className="grid-cols-1 gap-9 sm:grid-cols-2">
                <div className="flex flex-col gap-9">
                    {/* Edit Account Form */}
                    <div className="rounded-sm border border-stroke bg-white shadow-default dark:border-strokedark dark:bg-boxdark">
                        <div className="border-b border-stroke py-4 px-6.5 dark:border-strokedark">
                            <h3 className="font-medium text-black dark:text-white">Edit Account</h3>
                        </div>
                        <div className="flex flex-col gap-5.5 p-6.5">
                            {/* Username */}
                            <div>
                                <label className="mb-3 block text-black dark:text-white">Username</label>
                                <input
                                    type="text"
                                    name="username"
                                    value={accountDetails.username}
                                    onChange={handleInputChange}
                                    placeholder="Enter username"
                                    className="w-full rounded-lg border-[1.5px] border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary active:border-primary"
                                />
                            </div>

                            {/* Email */}
                            <div>
                                <label className="mb-3 block text-black dark:text-white">Email</label>
                                <input
                                    type="email"
                                    name="email"
                                    value={accountDetails.email}
                                    onChange={handleInputChange}
                                    placeholder="Enter email"
                                    className="w-full rounded-lg border-[1.5px] border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary active:border-primary"
                                />
                            </div>

                            {/* Phone Number */}
                            <div>
                                <label className="mb-3 block text-black dark:text-white">Phone Number</label>
                                <input
                                    type="text"
                                    name="phone"
                                    value={accountDetails.phone}
                                    onChange={handleInputChange}
                                    placeholder="Enter phone number (XXX-XXX-XXXX)"
                                    className="w-full rounded-lg border-[1.5px] border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary active:border-primary"
                                />
                            </div>

                            {/* Role */}
                            <div>
                                <label className="mb-3 block text-black dark:text-white">Role</label>
                                <select
                                    name="role"
                                    value={accountDetails.role}
                                    onChange={handleInputChange}
                                    className="w-full rounded-lg border-[1.5px] border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary active:border-primary"
                                >
                                    <option value="">Select Role</option>
                                    <option value="Admin">Admin</option>
                                    <option value="User">User</option>
                                </select>
                            </div>

                            {/* Status */}
                            <div>
                                <label className="mb-3 block text-black dark:text-white">Status</label>
                                <select
                                    name="status"
                                    value={accountDetails.status}
                                    onChange={handleInputChange}
                                    className="w-full rounded-lg border-[1.5px] border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary active:border-primary"
                                >
                                    <option value="">Select Status</option>
                                    <option value="Active">Active</option>
                                    <option value="Inactive">Inactive</option>
                                </select>
                            </div>

                            {/* Submit Button */}
                            <div className="flex justify-center p-6.5">
                                <button
                                    onClick={handleSubmit}
                                    className="rounded-md bg-blue-600 text-white px-6 py-3"
                                >
                                    Update Account
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </>
    );
};

export default EditAccount;
