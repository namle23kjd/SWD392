import React, { useState, useEffect } from "react";
import { toast } from "react-toastify"; // For displaying error messages

const OrderHistory: React.FC = () => {
    // State to store order data and selected status
    const [orders, setOrders] = useState<any[]>([]);
    const [searchTerm, setSearchTerm] = useState<string>(''); // State for search term
    const [editedOrders, setEditedOrders] = useState<any[]>([]); // Store orders with edited statuses

    // Fetching orders data (simulated, replace with actual API call)
    useEffect(() => {
        const fetchOrders = async () => {
            try {
                // Simulating an API request for fetching orders
                // Replace with your actual API request
                const fetchedOrders = [
                    {
                        orderId: "ORD001",
                        platform: "Platform A",
                        status: "In Progress",
                        orderDate: "2025-02-19",
                    },
                    {
                        orderId: "ORD002",
                        platform: "Platform B",
                        status: "Completed",
                        orderDate: "2025-02-18",
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

    // Handle status change for the order
    const handleStatusChange = (value: string, orderId: string) => {
        setEditedOrders(editedOrders.map(order =>
            order.orderId === orderId ? { ...order, status: value } : order
        ));
    };

    // Handle search functionality
    const handleSearch = (e: React.ChangeEvent<HTMLInputElement>) => {
        setSearchTerm(e.target.value);
    };

    // Filter orders based on the search term
    const filteredOrders = orders.filter(order =>
        order.orderId.toLowerCase().includes(searchTerm.toLowerCase()) ||
        order.platform.toLowerCase().includes(searchTerm.toLowerCase())
    );

    // Function to save edited order statuses
    const saveChanges = () => {
        try {
            // Simulating API call to save changes
            // Replace with your actual API request to save the changes
            setOrders(editedOrders);

            toast.success("Order statuses saved successfully!");
        } catch (error) {
            toast.error("Failed to save order statuses.");
        }
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
                <button
                    onClick={saveChanges}
                    className="bg-blue-600 text-white p-2 rounded-lg"
                >
                    Save
                </button>
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
                                        <select
                                            defaultValue={order.status}
                                            onChange={(e) => handleStatusChange(e.target.value, order.orderId)}
                                            className="border border-gray-300 p-2 rounded-lg text-sm focus:outline-none focus:border-blue-500"
                                        >
                                            <option value="In Progress">In Progress</option>
                                            <option value="Completed">Completed</option>
                                            <option value="In Transit">In Transit</option>
                                            <option value="Canceled">Canceled</option>
                                        </select>
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
        </div>
    );
};

export default OrderHistory;
