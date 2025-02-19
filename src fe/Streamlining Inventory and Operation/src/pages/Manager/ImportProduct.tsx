import React, { useState, ChangeEvent, FormEvent } from "react";
import Breadcrumb from '../../components/Breadcrumbs/Breadcrumb';
import SelectGroupTwo from '../../components/Forms/SelectGroup/SelectGroupTwo';
import DatePickerOne from '../../components/Forms/DatePicker/DatePickerOne';
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";

// Các kiểu dữ liệu cần thiết (Product, Supplier, Shelf)
interface ImportProductDetails {
    productId: string; // Mã sản phẩm
    quantity: number; // Số lượng nhập
    price: number; // Giá nhập
    unit: string; // Đơn vị
    supplyDate: string; // Ngày nhập kho
    supplier: string; // Nhà cung cấp
    shelf: string; // Kệ lưu trữ sản phẩm
    lot: string; // Lô sản phẩm
    note: string; // Ghi chú
    proof: string; // Hình ảnh chứng minh nhập kho
}

const ImportProduct: React.FC = () => {
    const [importDetails, setImportDetails] = useState<ImportProductDetails>({
        productId: "",
        quantity: 0,
        price: 0,
        unit: "unit",
        supplyDate: "",
        supplier: "",
        shelf: "",
        lot: "",
        note: "",
        proof: "",
    });

    const navigate = useNavigate();

    // Hàm xử lý thay đổi input
    const handleInputChange = (e: ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => {
        const { name, value } = e.target;
        setImportDetails((prev) => ({ ...prev, [name]: value }));
    };

    // Hàm xử lý thay đổi ngày tháng
    const handleDateChange = (date: string, field: string) => {
        setImportDetails((prev) => ({ ...prev, [field]: date }));
    };

    // Hàm xử lý khi chọn lot
    const handleLotChange = (e: ChangeEvent<HTMLSelectElement>) => {
        // Kiểm tra nếu shelf chưa được chọn thì không cho chọn lot
        if (!importDetails.shelf) {
            toast.error("Please select a shelf before selecting a lot.");
            return;
        }
        setImportDetails((prev) => ({ ...prev, lot: e.target.value }));
    };

    const handleSubmit = async (e: FormEvent) => {
        e.preventDefault();

        // Validate all required fields
        if (!importDetails.productId
            || !importDetails.quantity
            || !importDetails.price
            || !importDetails.supplyDate
            || !importDetails.supplier
            || !importDetails.shelf
            || !importDetails.lot) {
            toast.error("All fields are required except the note.");
            return;
        }

        // Validate quantity: greater than 0 and less than a max value (e.g. 10000)
        if (importDetails.quantity <= 0 || importDetails.quantity > 10000) {
            toast.error("Quantity must be greater than 0 and less than 10000.");
            return;
        }

        // Validate price: must be >= 0 and less than a max value (e.g. 99999999999999)
        if (importDetails.price < 0 || importDetails.price > 99999999999999) {
            toast.error("Price must be greater than or equal to 0 and less than 99999999999999.");
            return;
        }

        // Validate date: supply date must not be a future date
        const today = new Date();
        const supplyDate = new Date(importDetails.supplyDate);
        if (supplyDate > today) {
            toast.error("Supply date cannot be in the future.");
            return;
        }

        // Validate file type (proof)
        const proofFile = (document.querySelector('input[type="file"]') as HTMLInputElement).files?.[0];
        if (proofFile && proofFile.size > 10 * 1024 * 1024) { // 10MB
            toast.error("Proof file size must not exceed 10MB.");
            return;
        }

        if (proofFile && !["image/jpeg", "image/png", "application/pdf"].includes(proofFile.type)) {
            toast.error("Proof file must be an image (JPEG, PNG) or PDF.");
            return;
        }

        // If all validations passed, submit form
        try {
            // Gửi dữ liệu nhập kho tới API (cập nhật kho hoặc ghi lại giao dịch nhập kho)
            // await axios.post('/api/import', importDetails);
            alert("Product imported successfully!");
        } catch (error) {
            toast.error("Failed to import product");
        }
    };

    // Options mẫu cho SelectGroupTwo
    const productOptions = [
        { value: "product1", label: "Product 1" },
        { value: "product2", label: "Product 2" },
        { value: "product3", label: "Product 3" }
    ];

    const unitOptions = [
        { value: "vnd", label: "VND" },
        { value: "usd", label: "USD" },
        { value: "eur", label: "EUR" }
    ]

    const supplierOptions = [
        { value: "supplier1", label: "Supplier 1" },
        { value: "supplier2", label: "Supplier 2" },
        { value: "supplier3", label: "Supplier 3" }
    ];

    const shelfOptions = [
        { value: "shelf1", label: "Shelf 1" },
        { value: "shelf2", label: "Shelf 2" },
        { value: "shelf3", label: "Shelf 3" }
    ];

    const lotOptions = [
        { value: "lot1", label: "Lot 1" },
        { value: "lot2", label: "Lot 2" },
        { value: "lot3", label: "Lot 3" },
    ];

    return (
        <>
            <Breadcrumb pageName="Import Product to Warehouse" />

            <div className="grid-cols-1 gap-9 sm:grid-cols-2">
                <div className="flex flex-col gap-9">
                    {/* <!-- Product Information --> */}
                    <div className="rounded-sm border border-stroke bg-white shadow-default dark:border-strokedark dark:bg-boxdark">
                        <div className="border-b border-stroke py-4 px-6.5 dark:border-strokedark">
                            <h3 className="font-medium text-black dark:text-white">Import Product Information</h3>
                        </div>
                        <div className="flex flex-col gap-5.5 p-6.5">
                            {/* Chọn sản phẩm */}
                            <div>
                                <SelectGroupTwo
                                    label="Product"
                                    name="productId"
                                    value={importDetails.productId}
                                    onChange={handleInputChange}
                                    options={productOptions}
                                />
                            </div>

                            {/* Số lượng */}
                            <div>
                                <label className="mb-3 block text-black dark:text-white">Quantity</label>
                                <input
                                    type="number"
                                    name="quantity"
                                    value={importDetails.quantity}
                                    onChange={handleInputChange}
                                    placeholder="Enter quantity"
                                    className="w-full rounded-lg border-[1.5px] border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary active:border-primary"
                                />
                            </div>

                            {/* Giá nhập */}
                            <div>
                                <label className="mb-3 block text-black dark:text-white">Price</label>
                                <input
                                    type="number"
                                    name="price"
                                    value={importDetails.price}
                                    onChange={handleInputChange}
                                    placeholder="Enter price"
                                    className="w-full rounded-lg border-[1.5px] border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary active:border-primary"
                                />
                            </div>

                            <div>
                                <SelectGroupTwo
                                    label="Unit Price"
                                    name="unit"
                                    value={importDetails.unit}
                                    onChange={handleInputChange}
                                    options={unitOptions}
                                />
                            </div>

                            {/* Ngày nhập kho */}
                            <div>
                                <label className="mb-3 block text-black dark:text-white">Supply Date</label>
                                <DatePickerOne
                                    selectedDate={importDetails.supplyDate}
                                    onChange={(date: string) => handleDateChange(date, "supplyDate")}
                                />
                            </div>

                            {/* Nhà cung cấp */}
                            <div>
                                <SelectGroupTwo
                                    label="Supplier"
                                    name="supplier"
                                    value={importDetails.supplier}
                                    onChange={handleInputChange}
                                    options={supplierOptions}
                                />
                            </div>

                            <div>
                                <label className="mb-3 block text-black dark:text-white">
                                    Export product note
                                </label>
                                <textarea
                                    value={importDetails.note}
                                    onChange={handleInputChange}
                                    rows={6}
                                    placeholder="Note"
                                    className="w-full rounded-lg border-[1.5px] border-stroke bg-transparent py-3 px-5 text-black outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:text-white dark:focus:border-primary"
                                ></textarea>
                            </div>

                            {/* Kệ */}
                            <div>
                                <SelectGroupTwo
                                    label="Shelf"
                                    name="shelf"
                                    value={importDetails.shelf}
                                    onChange={handleInputChange}
                                    options={shelfOptions}
                                />
                            </div>

                            {/* Chọn lô */}
                            <div>
                                <SelectGroupTwo
                                    label="Lot"
                                    name="lot"
                                    value={importDetails.lot}
                                    onChange={handleLotChange}
                                    options={lotOptions}
                                />
                            </div>

                            <div>
                                <label className="mb-3 block text-black dark:text-white">
                                    Proof of Export Product
                                </label>
                                <input
                                    type="file"
                                    className="w-full cursor-pointer rounded-lg border-[1.5px] border-stroke bg-transparent outline-none transition file:mr-5 file:border-collapse file:cursor-pointer file:border-0 file:border-r file:border-solid file:border-stroke file:bg-whiter file:py-3 file:px-5 file:hover:bg-primary file:hover:bg-opacity-10 focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:file:border-form-strokedark dark:file:bg-white/30 dark:file:text-white dark:focus:border-primary"
                                />
                            </div>

                            <div className="flex justify-center p-6.5">
                                <button
                                    onClick={handleSubmit}
                                    className="rounded-md bg-blue-600 text-white px-6 py-3"
                                >
                                    Import Product
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </>
    );
};

export default ImportProduct;
