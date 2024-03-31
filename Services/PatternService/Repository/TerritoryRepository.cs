using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using northwindAPI.Data.ModelDTO;
using northwindAPI.Models;

namespace northwindAPI.PatternService.Repository
{
    public class TerritoryRepository
    {
        private readonly AppDbContext _context;
        public TerritoryRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Territory?> GetByIdAsync(string? id)
        {
            var record =await _context.Set<Territory>().FirstOrDefaultAsync(x=>x.TerritoryId==id);
            return record;
        }
        public async Task UpdateAsync(string id,Territory territory)
        {
            var record = await _context.Set<Territory>().FirstOrDefaultAsync(
                x=>x.TerritoryId==id
            );
            _context.Entry(record).CurrentValues.SetValues(territory);
        }
        public async Task DeleteAsync(string id)
        {
            var record =await _context.Set<Territory>().AsNoTracking().FirstOrDefaultAsync(
                x=>x.TerritoryId==id
            );
            _context.Remove(record);
        }
    }
}