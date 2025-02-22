import React, { useState } from 'react';
import Breadcrumb from '../components/Breadcrumbs/Breadcrumb';
import { useNavigate } from 'react-router-dom';

interface Shelf {
  id: number;
  code: string;
  name: string;
  location: string;
  capacity: number;
}

const Shelfs: React.FC = () => {
  const navigate = useNavigate();
  const [search, setSearch] = useState<string>('');
  const [shelfs, setShelfs] = useState<Shelf[]>([
    {
      id: 1,
      code: 'S001',
      name: 'Aba',
      location: 'A1',
      capacity: 10,

    },
    {
      id: 2,
      code: 'S002',
      name: 'Anta',
      location: 'A2',
      capacity: 10,
    },
    {
      id: 3,
      code: 'S003',
      name: 'Befa',
      location: 'A3',
      capacity: 10,
    },
    {
      id: 4,
      code: 'S004',
      name: 'Naga',
      location: 'A4',
      capacity: 10,
    },
  ]);
  const handleSearch = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === 'Enter') {
      console.log('search shelf', search);
    }
  }
  return (
    <>
      <Breadcrumb pageName="Shelfs Manage" />
      <div className="p-4  relative">
        <button className="mb-4 p-2 bg-blue-500 text-white rounded hover:-translate-y-1 hover:shadow-lg transition-transform">
          Create Shelf
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
              <th className="border p-2 bg-gray-200" >Actions</th>
            </tr>
          </thead>
          <tbody>
            {shelfs.map((shelf) => (
              <tr key={shelf.id}>
               
                <td className="border p-2 text-center"> {shelf.id}</td>
                <td className="border p-2 text-center">{shelf.code}</td>
                <td className="border p-2 text-center" >{shelf.name}</td>
                <td className="border p-2 text-center">{shelf.location}</td>
                <td className="border p-2 text-center">{shelf.capacity}</td>
                <td className="border p-2 flex justify-center ">
                  <button className="p-2 bg-yellow-500 text-white rounded mr-2 hover:-translate-y-1 hover:shadow-lg transition-transform">
                    Update
                  </button>
                  <button className="p-2 mr-2 bg-red-500 text-white rounded hover:-translate-y-1 hover:shadow-lg transition-transform">
                    Delete
                  </button>
                  <button onClick={() => navigate(`/lots?shelfId=${shelf.id}`)} className="p-2 bg-green-500 text-white rounded hover:-translate-y-1 hover:shadow-lg transition-transform">
                    View Lots
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </>
  );
};

export default Shelfs;
