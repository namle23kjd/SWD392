import React from "react";
import Breadcrumb from "../components/Breadcrumbs/Breadcrumb";

const Products = () => {
  return (
    <>
      <Breadcrumb pageName="Product Manage" />
      <div className="container mx-auto p-6">
        {/* Form Thêm Sản Phẩm */}
        <div className="bg-white p-6 rounded-lg shadow-md">
          <h2 className="text-lg font-semibold mb-4">Add Product</h2>
          <form className="grid grid-cols-2 gap-4">
            <input
              type="text"
              placeholder="SKU"
              className="border p-2 rounded w-full"
            />
            <input
              type="text"
              placeholder="Barcode"
              className="border p-2 rounded w-full"
            />
            <input
              type="text"
              placeholder="Product Name"
              className="border p-2 rounded w-full"
            />
            <input
              type="text"
              placeholder="Created By"
              className="border p-2 rounded w-full"
            />
            <textarea
              placeholder="Description"
              className="border p-2 rounded w-full col-span-2"
            />
            <button
              type="submit"
              className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600 col-span-2"
            >
              Add Product
            </button>
          </form>
        </div>

        {/* Danh sách sản phẩm */}
        <div className="mt-6">
          <h2 className="text-xl font-semibold mb-2">Product List</h2>
          <table className="w-full border-collapse border border-gray-300">
            <thead>
              <tr className="bg-gray-100">
                <th className="border p-2">ID</th>
                <th className="border p-2">SKU</th>
                <th className="border p-2">Barcode</th>
                <th className="border p-2">Name</th>
                <th className="border p-2">Description</th>
                <th className="border p-2">Created By</th>
                <th className="border p-2">Actions</th>
              </tr>
            </thead>
            <tbody>
              <tr className="border">
                <td className="border p-2">1</td>
                <td className="border p-2">ABC123</td>
                <td className="border p-2">123456789</td>
                <td className="border p-2">Product Name</td>
                <td className="border p-2">Sample description</td>
                <td className="border p-2">Admin</td>
                <td className="border p-2 space-x-2">
                  <button className="text-blue-500">Edit</button>
                  <button className="text-red-500">Delete</button>
                </td>
              </tr>
              <tr>
                <td colSpan={7} className="text-center py-2">
                  No Products Available
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </>
  );
};

export default Products;
