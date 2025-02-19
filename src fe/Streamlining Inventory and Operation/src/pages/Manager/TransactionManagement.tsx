import React, { useState, useEffect, ChangeEvent } from 'react';
import Breadcrumb from '../../components/Breadcrumbs/Breadcrumb';
import { useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';

// Dữ liệu mẫu cho các bảng
const sampleTransactions = [
    { id: 'T001', date: '2025-02-01', amount: 100, status: 'Completed' },
    { id: 'T002', date: '2025-02-02', amount: 200, status: 'Pending' },
    { id: 'T003', date: '2025-02-03', amount: 150, status: 'Completed' },
];

const sampleSubscriptions = [
    { id: 'S001', user: 'John Doe', plan: 'Premium', status: 'Active' },
    { id: 'S002', user: 'Jane Smith', plan: 'Standard', status: 'Active' },
    { id: 'S003', user: 'Mike Johnson', plan: 'Basic', status: 'Inactive' },
];

const TransactionManagement: React.FC = () => {
    const [transactions, setTransactions] = useState(sampleTransactions);
    const [subscriptions, setSubscriptions] = useState(sampleSubscriptions);
    const [selectedYearTransaction, setSelectedYearTransaction] = useState<string>(''); // Năm giao dịch
    const [selectedMonthTransaction, setSelectedMonthTransaction] = useState<string>(''); // Tháng giao dịch
    const [selectedDayTransaction, setSelectedDayTransaction] = useState<string>(''); // Ngày giao dịch

    const [selectedYearSubscription, setSelectedYearSubscription] = useState<string>(''); // Năm đăng ký
    const [selectedMonthSubscription, setSelectedMonthSubscription] = useState<string>(''); // Tháng đăng ký
    const [selectedDaySubscription, setSelectedDaySubscription] = useState<string>(''); // Ngày đăng ký

    const [alertMessage, setAlertMessage] = useState<string | null>(null);
    const [reportDate, setReportDate] = useState<string>(''); // Ngày cho báo cáo
    const navigate = useNavigate();

    useEffect(() => {
        const expiredCard = checkExpiredPayments();
        if (expiredCard) {
            setAlertMessage(`Alert: The payment for ${expiredCard.user} is expired.`);
        }
    }, []);

    const getStatusColor = (status: string) => {
        switch (status) {
            case 'Active':
                return 'text-green-800';
            case 'Pending':
                return 'text-yellow-800';
            case 'Completed':
                return 'text-blue-800';
            case 'Inactive':
                return 'text-red-800';
            default:
                return 'text-gray-800';
        }
    };

    const checkExpiredPayments = () => {
        return { user: 'John Doe' };
    };

    // Hàm kiểm tra ngày hợp lệ
    const isValidDay = (year: string, month: string, day: string): boolean => {
        const daysInMonth = new Date(Number(year), Number(month), 0).getDate();
        return Number(day) <= daysInMonth;
    };

    // Hàm validate cho từng form
    const validateFormTransaction = () => {
        if (!selectedYearTransaction || !selectedMonthTransaction || !selectedDayTransaction) {
            toast.error("Please select Year, Month, and Day for Transaction.");
            return false;
        }
        if (Number(selectedMonthTransaction) === 2 && Number(selectedDayTransaction) > 29) {
            toast.error("February can only have up to 29 days.");
            return false;
        }
        if (!isValidDay(selectedYearTransaction, selectedMonthTransaction, selectedDayTransaction)) {
            toast.error("Invalid day selected for this month.");
            return false;
        }
        return true;
    };

    const validateFormSubscription = () => {
        if (!selectedYearSubscription || !selectedMonthSubscription || !selectedDaySubscription) {
            toast.error("Please select Year, Month, and Day for Subscription.");
            return false;
        }
        if (Number(selectedMonthSubscription) === 2 && Number(selectedDaySubscription) > 29) {
            toast.error("February can only have up to 29 days.");
            return false;
        }
        if (!isValidDay(selectedYearSubscription, selectedMonthSubscription, selectedDaySubscription)) {
            toast.error("Invalid day selected for this month.");
            return false;
        }
        return true;
    };

    // Hàm xử lý khi chọn năm, tháng, ngày cho các bảng
    const handleDateChangeTransaction = () => {
        if (validateFormTransaction()) {
            const fullDate = `${selectedYearTransaction}-${selectedMonthTransaction}-${selectedDayTransaction}`;
            setReportDate(fullDate);
        }
    };

    const handleDateChangeSubscription = () => {
        if (validateFormSubscription()) {
            const fullDate = `${selectedYearSubscription}-${selectedMonthSubscription}-${selectedDaySubscription}`;
            setReportDate(fullDate);
        }
    };

    return (
        <>
            <Breadcrumb pageName="Transaction Management" />

            <div className="grid-cols-1 gap-9 sm:grid-cols-2">
                <div className="flex flex-col gap-9">
                    {/* Subscription Management */}
                    <div className="rounded-sm border border-stroke bg-white shadow-default dark:border-strokedark dark:bg-boxdark">
                        <div className="border-b border-stroke py-4 px-6.5 dark:border-strokedark">
                            <h3 className="font-medium text-black dark:text-white">View Subscriptions</h3>
                        </div>
                        <div className="flex flex-col gap-5.5 p-6.5">
                            {/* Select Year, Month, Day for Subscription */}
                            <div className="flex gap-4 mb-4">
                                <select
                                    className="w-full rounded-lg border-[1.5px] border-stroke bg-transparent py-1 px-5 text-black outline-none transition focus:border-primary dark:border-form-strokedark dark:bg-form-input dark:focus:border-primary"
                                    value={selectedYearSubscription}
                                    onChange={(e) => setSelectedYearSubscription(e.target.value)}
                                >
                                    <option value="">Select Year</option>
                                    <option value="2025">2025</option>
                                    <option value="2024">2024</option>
                                </select>

                                <select
                                    className="w-full rounded-lg border-[1.5px] border-stroke bg-transparent py-1 px-5 text-black outline-none transition focus:border-primary dark:border-form-strokedark dark:bg-form-input dark:focus:border-primary"
                                    value={selectedMonthSubscription}
                                    onChange={(e) => setSelectedMonthSubscription(e.target.value)}
                                >
                                    <option value="">Select Month</option>
                                    <option value="01">January</option>
                                    <option value="02">February</option>
                                    {/* Add other months */}
                                </select>

                                <select
                                    className="w-full rounded-lg border-[1.5px] border-stroke bg-transparent py-1 px-5 text-black outline-none transition focus:border-primary dark:border-form-strokedark dark:bg-form-input dark:focus:border-primary"
                                    value={selectedDaySubscription}
                                    onChange={(e) => setSelectedDaySubscription(e.target.value)}
                                >
                                    <option value="">Select Day</option>
                                    <option value="01">1</option>
                                    <option value="02">2</option>
                                    {/* Add other days */}
                                </select>

                                <button
                                    className="min-w-40 bg-blue-600 text-white py-2 px-4 rounded-md"
                                    onClick={handleDateChangeSubscription}
                                >
                                    Generate Data
                                </button>

                                <button
                                    className="min-w-40 bg-green-600 text-white py-2 px-4 rounded-md"
                                    onClick={handleDateChangeSubscription}
                                >
                                    Export Data
                                </button>
                            </div>

                            <table className="w-full">
                                <thead>
                                    <tr>
                                        <th className="px-4 py-2">Subscription ID</th>
                                        <th className="px-4 py-2">User</th>
                                        <th className="px-4 py-2">Plan</th>
                                        <th className="px-4 py-2">Status</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {subscriptions.map((subscription) => (
                                        <tr key={subscription.id}>
                                            <td className="px-4 py-2 text-center">{subscription.id}</td>
                                            <td className="px-4 py-2 text-center">{subscription.user}</td>
                                            <td className="px-4 py-2 text-center">{subscription.plan}</td>
                                            <td className={`px-4 py-2 text-center ${getStatusColor(subscription.status)}`}>
                                                {subscription.status}
                                            </td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                        </div>
                    </div>

                    {/* Transaction Management */}
                    <div className="rounded-sm border border-stroke bg-white shadow-default dark:border-strokedark dark:bg-boxdark">
                        <div className="border-b border-stroke py-4 px-6.5 dark:border-strokedark">
                            <h3 className="font-medium text-black dark:text-white">View Transactions</h3>
                        </div>
                        <div className="flex flex-col gap-5.5 p-6.5">
                            {/* Select Year, Month, Day for Transaction */}
                            <div className="flex gap-4 mb-4">
                                <select
                                    className="w-full rounded-lg border-[1.5px] border-stroke bg-transparent py-1 px-5 text-black outline-none transition focus:border-primary dark:border-form-strokedark dark:bg-form-input dark:focus:border-primary"
                                    value={selectedYearTransaction}
                                    onChange={(e) => setSelectedYearTransaction(e.target.value)}
                                >
                                    <option value="">Select Year</option>
                                    <option value="2025">2025</option>
                                    <option value="2024">2024</option>
                                </select>

                                <select
                                    className="w-full rounded-lg border-[1.5px] border-stroke bg-transparent py-1 px-5 text-black outline-none transition focus:border-primary dark:border-form-strokedark dark:bg-form-input dark:focus:border-primary"
                                    value={selectedMonthTransaction}
                                    onChange={(e) => setSelectedMonthTransaction(e.target.value)}
                                >
                                    <option value="">Select Month</option>
                                    <option value="01">January</option>
                                    <option value="02">February</option>
                                    {/* Add other months */}
                                </select>

                                <select
                                    className="w-full rounded-lg border-[1.5px] border-stroke bg-transparent py-1 px-5 text-black outline-none transition focus:border-primary dark:border-form-strokedark dark:bg-form-input dark:focus:border-primary"
                                    value={selectedDayTransaction}
                                    onChange={(e) => setSelectedDayTransaction(e.target.value)}
                                >
                                    <option value="">Select Day</option>
                                    <option value="01">1</option>
                                    <option value="02">2</option>
                                    {/* Add other days */}
                                </select>

                                <button
                                    className="min-w-40 bg-blue-600 text-white py-2 px-4 rounded-md"
                                    onClick={handleDateChangeTransaction}
                                >
                                    Generate Data
                                </button>

                                <button
                                    className="min-w-40 bg-green-600 text-white py-2 px-4 rounded-md"
                                    onClick={handleDateChangeTransaction}
                                >
                                    Export Data
                                </button>
                            </div>

                            <table className="w-full">
                                <thead>
                                    <tr>
                                        <th className="px-4 py-2">Transaction ID</th>
                                        <th className="px-4 py-2">Date</th>
                                        <th className="px-4 py-2">Amount</th>
                                        <th className="px-4 py-2">Status</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {transactions.map((transaction) => (
                                        <tr key={transaction.id}>
                                            <td className="px-4 py-2 text-center">{transaction.id}</td>
                                            <td className="px-4 py-2 text-center">{transaction.date}</td>
                                            <td className="px-4 py-2 text-center">${transaction.amount}</td>
                                            <td className={`px-4 py-2 text-center ${getStatusColor(transaction.status)}`}>
                                                {transaction.status}
                                            </td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </>
    );
};

export default TransactionManagement;
