import React, { useState, useEffect } from "react";
import { toast } from "react-toastify"; // For displaying error messages
import { DeleteOutlined } from '@ant-design/icons'; // Import delete icon from Ant Design
import { getListPlatforms, getListProducts, getListSuppliers } from "../fetch/order";
import { createOrder } from "../fetch/order"; // Import the createOrder API function
import { accountListLoader } from "../fetch/account";
import { getUserInfo } from "../util/auth";
import { useNavigate } from "react-router-dom";

const CreateOrder: React.FC = () => {
    // State for the order fields
    const navigate = useNavigate()
    const [orderDetails, setOrderDetails] = useState({
        platformId: "",
        orderDate: "",
        supplierId: "",
        totalPrice: 0,
        status: "In Progress",
    });

    // State for the products in the order
    const [orderItems, setOrderItems] = useState<any[]>([]);

    // Simulating data for products, platforms, and suppliers
    const [products, setProducts] = useState<any[]>([]);
    const [platforms, setPlatforms] = useState<any[]>([]);
    const [suppliers, setSuppliers] = useState<any[]>([]);
    const [userId, setUserId] = useState(null);

    async function fetchUserId() {
        const response = await accountListLoader();
        if (response.statusCode === 200) {
            const listAccount = response.result;
            const userIdFetch = listAccount.find((account: { email: string }) =>
                account.email === getUserInfo()?.email).id;
            setUserId(userIdFetch);
        }
    }

    useEffect(() => {
        fetchUserId();
    }, []);

    useEffect(() => {
        // Simulate fetching product, platform, and supplier data
        const fetchData = async () => {
            const platformData = await getListPlatforms();
            setPlatforms(platformData.result.map((platformItem: { platformId: number, name: string }) => ({
                id: platformItem.platformId,
                name: platformItem.name
            })));

            const productData = await getListProducts();
            setProducts(productData.result.products.map((productData: { sku: string, productName: string, basePrice: number, productId: string }) => ({
                id: productData.sku,
                name: productData.productName,
                price: productData.basePrice,
                productId: productData.productId
            })));

            const supplierData = await getListSuppliers();
            setSuppliers(supplierData.result.suppliers.map((supplierItem: { supplierId: number, name: string }) => ({
                id: supplierItem.supplierId,
                name: supplierItem.name
            })));
        };

        fetchData();
    }, []);

    // Handle input changes for order details
    const handleOrderDetailsChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
        const { name, value } = e.target;
        setOrderDetails((prev) => ({ ...prev, [name]: value }));
    };

    const handleProductChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>, index: number) => {
        const { name, value } = e.target;
        const updatedItems = [...orderItems];

        if (name === "productId") {
            // Tìm thông tin sản phẩm từ danh sách sản phẩm theo productId
            const selectedProduct = products.find(product => product.id === value);

            // Cập nhật lại giá cho sản phẩm trong orderItems
            if (selectedProduct) {
                updatedItems[index] = {
                    ...updatedItems[index],
                    [name]: value,
                    price: selectedProduct.price,  // Gắn giá sản phẩm vào orderItems
                    totalPrice: updatedItems[index].quantity * selectedProduct.price,  // Tính lại tổng giá
                    id: selectedProduct.productId
                };
            }
        } else if (name === "quantity") {
            // Cập nhật lại tổng giá khi thay đổi số lượng
            updatedItems[index].totalPrice = updatedItems[index].quantity * updatedItems[index].price;
        } else {
            updatedItems[index] = { ...updatedItems[index], [name]: value };
        }

        // Cập nhật lại orderItems
        setOrderItems(updatedItems);

        // Cập nhật tổng giá trị đơn hàng
        calculateTotalPrice(updatedItems);
    };

    const addProduct = () => {
        setOrderItems([
            ...orderItems,
            {
                productId: "",
                quantity: 1,
                price: 0,  // Mặc định giá là 0, nhưng sẽ được cập nhật khi chọn sản phẩm
                totalPrice: 0,
                notes: "",
            },
        ]);
    };

    // Function to delete a product from the order items
    const deleteProduct = (index: number) => {
        const updatedItems = orderItems.filter((_, idx) => idx !== index);
        setOrderItems(updatedItems);

        // Recalculate overall total price
        calculateTotalPrice(updatedItems);
    };

    // Function to calculate the total price of the order
    const calculateTotalPrice = (updatedItems: any[]) => {
        const newTotalPrice = updatedItems.reduce((acc, item) => acc + item.totalPrice, 0);
        setOrderDetails((prev) => ({ ...prev, totalPrice: newTotalPrice }));
    };

    // Handle form submission (simulating an API call)
    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        // Validation
        if (!orderDetails.platformId || !orderDetails.supplierId || !orderDetails.orderDate) {
            toast.error("Please fill in all required fields.");
            return;
        }

        if (orderItems.length === 0 || orderItems.some((item) => !item.productId || !item.quantity)) {
            toast.error("Please add products with valid quantities.");
            return;
        }

        try {
            // Prepare the order data for submission
            console.log(orderItems)
            const submitData = {
                userId: userId,
                platformId: parseInt(orderDetails.platformId),
                platformOrderId: "string",
                orderDate: new Date(orderDetails.orderDate).toISOString(),
                orderItems: orderItems.map(item => ({
                    productId: parseInt(item.id),
                    quantity: item.quantity,
                })),
            };

            // Call the createOrder API
            const response = await createOrder(submitData);

            if (response.statusCode === 201) {
                navigate("/manager/order-history")
                toast.success("Order created successfully!");
            } else {
                toast.error("Failed to create order.");
            }
        } catch (error) {
            toast.error("An error occurred while creating the order.");
        }
    };

    const getAvailableProducts = (index: number) => {
        const selectedProductIds = orderItems.map((item, idx) => (idx !== index ? item.productId : null));
        return products.filter((product) => !selectedProductIds.includes(product.id));
    };

    return (
        <div className="p-6 bg-gray-50">
            <h3 className="text-xl font-semibold text-gray-900">Create New Order</h3>
            <form onSubmit={handleSubmit} className="space-y-6 mt-6">
                {/* Platform */}
                <div>
                    <label className="block text-gray-600">Platform</label>
                    <select
                        name="platformId"
                        value={orderDetails.platformId}
                        onChange={handleOrderDetailsChange}
                        className="w-full border rounded-lg px-4 py-2 mt-1"
                    >
                        <option value="">Select Platform</option>
                        {platforms.map((platform) => (
                            <option key={platform.id} value={platform.id}>
                                {platform.name}
                            </option>
                        ))}
                    </select>
                </div>

                {/* Order Date */}
                <div>
                    <label className="block text-gray-600">Order Date</label>
                    <input
                        type="date"
                        name="orderDate"
                        value={orderDetails.orderDate}
                        onChange={handleOrderDetailsChange}
                        className="w-full border rounded-lg px-4 py-2 mt-1"
                    />
                </div>

                {/* Status */}
                <div>
                    <label className="block text-gray-600">Status</label>
                    <select
                        name="status"
                        value={orderDetails.status}
                        onChange={handleOrderDetailsChange}
                        className="w-full border rounded-lg px-4 py-2 mt-1"
                    >
                        <option value="In Progress">In Progress</option>
                        <option value="Completed">Completed</option>
                    </select>
                </div>

                {/* Supplier */}
                <div>
                    <label className="block text-gray-600">Supplier</label>
                    <select
                        name="supplierId"
                        value={orderDetails.supplierId}
                        onChange={handleOrderDetailsChange}
                        className="w-full border rounded-lg px-4 py-2 mt-1"
                    >
                        <option value="">Select Supplier</option>
                        {suppliers.map((supplier) => (
                            <option key={supplier.id} value={supplier.id}>
                                {supplier.name}
                            </option>
                        ))}
                    </select>
                </div>

                {/* Add Product Table */}
                <div className="mt-6">
                    <h4 className="text-lg font-semibold text-gray-900">Add Products</h4>
                    <button
                        type="button"
                        onClick={addProduct}
                        className="bg-blue-600 text-white py-2 px-4 rounded-lg mt-4"
                    >
                        Add Product
                    </button>

                    {orderItems.map((item, index) => (
                        <div key={index} className="mt-4 space-y-4">
                            <div className="flex items-center gap-4">
                                <select
                                    name="productId"
                                    value={item.productId}
                                    onChange={(e) => handleProductChange(e, index)}
                                    className="w-1/3 border rounded-lg px-4 py-2"
                                >
                                    <option value="">Select Product</option>
                                    {getAvailableProducts(index).map((product) => (
                                        <option key={product.id} value={product.id}>
                                            {product.name} - ${product.price}
                                        </option>
                                    ))}
                                </select>

                                <input
                                    type="number"
                                    name="quantity"
                                    value={item.quantity}
                                    onChange={(e) => handleProductChange(e, index)}
                                    className="w-1/6 border rounded-lg px-4 py-2"
                                    min="1"
                                />

                                <input
                                    type="text"
                                    name="notes"
                                    value={item.notes}
                                    onChange={(e) => handleProductChange(e, index)}
                                    placeholder="Notes"
                                    className="w-1/3 border rounded-lg px-4 py-2"
                                />

                                <input
                                    type="text"
                                    value={item.totalPrice}
                                    readOnly
                                    className="w-1/6 border rounded-lg px-4 py-2"
                                />

                                <DeleteOutlined
                                    onClick={() => deleteProduct(index)} // Call deleteProduct when clicked
                                    className="text-red-600 cursor-pointer"
                                />
                            </div>
                        </div>
                    ))}
                </div>

                {/* Display Total Price */}
                <div className="mt-4">
                    <label className="block text-gray-600 font-semibold">Total Price</label>
                    <input
                        type="text"
                        value={`$${orderDetails.totalPrice.toFixed(2)}`} // Format the price with two decimal places
                        readOnly
                        className="w-full border rounded-lg px-4 py-2 mt-1 bg-gray-100 text-gray-600"
                    />
                </div>

                {/* Submit Button */}
                <div className="mt-6">
                    <button
                        type="submit"
                        className="w-full bg-blue-600 text-white py-2 rounded-lg"
                    >
                        Create Order
                    </button>
                </div>
            </form>
        </div>
    );
};

export default CreateOrder;
