using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using northwindAPI.Models;

namespace northwindAPI.PatternService.Repository
{
    public class OrderDetailsRepository
    {
        private readonly AppDbContext _context;
        public OrderDetailsRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IQueryable<OrderDetail>> GetByIdAsync(int orderId)
        {
            var records =_context.Set<OrderDetail>().Where(x=>x.OrderId == orderId).AsQueryable();
            return records;
        }
    }
}