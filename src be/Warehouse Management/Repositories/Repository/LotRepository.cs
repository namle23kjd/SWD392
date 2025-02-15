﻿using Microsoft.EntityFrameworkCore;
using Warehouse_Management.Data;
using Warehouse_Management.Models.Domain;
using Warehouse_Management.Repositories.IRepository;

namespace Warehouse_Management.Repositories.Repository
{
    public class LotRepository : ILotRepository
    {
        private readonly WareHouseDbContext _db;
        public LotRepository(WareHouseDbContext db)
        {
            _db = db;
        }
        public async Task CreateAsync(Lot lot)
        {
            await _db.Lots.AddAsync(lot);
        }

        public async Task<IEnumerable<Lot>> GetAllAsync()
        {
            return await _db.Lots
            .Include(l => l.Product)
            .Include(l => l.Shelf)
            .ToListAsync();
        }

        public async Task<Lot?> GetByIdAsync(int id)
        {
            return await  _db.Lots
            .Include(l => l.Product)
            .Include(l => l.Shelf)
            .FirstOrDefaultAsync(l => l.LotId == id);
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Lot lot)
        {
            _db.Lots.Update(lot);
        }
    }
}
