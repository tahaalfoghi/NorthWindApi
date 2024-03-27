using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using northwindAPI.Data.ModelDTO;
using northwindAPI.Models;

namespace northwindAPI.PatternService.Repository
{
    public class CustomerRepository
    {
        private readonly AppDbContext _context;
        public CustomerRepository(AppDbContext context)
        {
            _context = context;
        }
        public  async Task UpdateAsync(string? id, Customer customer)
        {
            
            var record =await _context.Set<Customer>().FirstOrDefaultAsync(x=>x.CustomerId == id);
            _context.Entry(record).CurrentValues.SetValues(customer);
        }
        public async Task DeleteAsync(string? id)
        {
            var record = await _context.Set<Customer>().FirstOrDefaultAsync(x=>x.CustomerId == id);
            _context.Remove(record);
        }
    }
}