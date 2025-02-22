import React, { useState } from 'react'
import Breadcrumb from '../components/Breadcrumbs/Breadcrumb'
import { useNavigate } from 'react-router-dom';

interface Lot {
  id: number;
  productId: number;
  shelfId: number;
  code: string;
  name: string;
  location: string;
  capacity: number;
}


const Lots:React.FC = () => {
  const navigate = useNavigate();
  const [search, setSearch] = useState<string>('');
  const [lots,setLots] = useState<Lot[]>([
    {
      id: 1,
      productId: 1,
      shelfId: 1,
      code: 'L001',
      name: 'A',
      location: 'A14',
      capacity: 10,
    },
    {
      id: 2,
      productId: 1,
      shelfId: 2,
      code: 'L003',
      name: 'B',
      location: 'A31',
      capacity: 10,
    },
    {
      id: 3,
      productId: 3,
      shelfId: 2,
      code: 'L004',
      name: 'C',
      location: 'A12',
      capacity: 10,
    },
    {
      id: 4,
      productId: 4,
      shelfId: 1,
      code: 'L001',
      name: 'D',
      location: 'A11',
      capacity: 10,
    },
  ])
  const handleSearch = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === 'Enter') {
      console.log('search lots', search);
    }
  }
  return (

    <>
    <Breadcrumb pageName="Lots" />
     <div className="p-4  relative">
        <button className="mb-4 p-2 bg-blue-500 text-white rounded hover:-translate-y-1 hover:shadow-lg transition-transform">
          Create Lots
        </button>
        <input
          type="text"
          placeholder="Search by Code, Name,..."
          onChange={(e) => setSearch(e.target.value)}
          value={search}
          onKeyDown={handleSearch}
          className="p-2 border border-gray-400  rounded w-2/5 absolute right-4"
        />
        <table className="w-full  table-auto border-collapse">
          <thead>
            <tr>
              <th className="border p-2 bg-yellow-100">ID</th>
              <th className="border p-2 bg-gray-200">Code</th>
              <th className="border p-2 bg-gray-200">Name</th>
              <th className="border p-2 bg-gray-200">Location</th>
              <th className="border p-2 bg-gray-200">Capacity</th>
              <th className="border p-2 bg-gray-200">Shelf</th>
              <th className="border p-2 bg-gray-200" >Actions</th>
            </tr>
          </thead>
          <tbody>
            {lots.map((lot) => (
              <tr key={lot.id}>
               
                <td className="border p-2 text-center"> {lot.id}</td>
                <td className="border p-2 text-center">{lot.code}</td>
                <td className="border p-2 text-center" >{lot.name}</td>
                <td className="border p-2 text-center">{lot.location}</td>
                <td className="border p-2 text-center">{lot.capacity}</td>
                <td className="border p-2 text-center">{lot.shelfId}</td>
                <td className="border p-2 flex justify-center ">
                  <button className="p-2 bg-yellow-500 text-white rounded mr-2 hover:-translate-y-1 hover:shadow-lg transition-transform">
                    Update
                  </button>
                  <button className="p-2 mr-2 bg-red-500 text-white rounded hover:-translate-y-1 hover:shadow-lg transition-transform">
                    Delete
                  </button>
                  <button onClick={() => navigate(`/products?lotId=${lot.id}&shelfId=${lot.shelfId}`)} className="p-2 bg-green-500 text-white rounded hover:-translate-y-1 hover:shadow-lg transition-transform">
                    View Products
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </>
  )
}

export default Lots