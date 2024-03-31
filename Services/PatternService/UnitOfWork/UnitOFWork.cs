using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;
using northwindAPI.Models;
using northwindAPI.PatternService.Repository;
using northwindAPI.RepositoryService;

namespace northwindAPI.PatternService.UnitOfWork
{
    public sealed class UnitOFWork:IUnitOfWork
    {
        private readonly AppDbContext _context;
        public CustomerRepository customerRepository  {get;}
        public OrderDetailsRepository orderDetailsRepository {get;}
        public TerritoryRepository territoryRepository {get;}
        public UnitOFWork(AppDbContext context, CustomerRepository customerRepository, 
        OrderDetailsRepository orderDetailsRepository, 
        TerritoryRepository territoryRepository)
        {
            _context = context;
            this.customerRepository = customerRepository;
            this.orderDetailsRepository = orderDetailsRepository;
            this.territoryRepository = territoryRepository;
        }
        public void Dispose()
        {
            _context.Dispose();
        }
        public async Task SaveChangeAsync(CancellationToken token)
        {
            await _context.SaveChangesAsync(token);
        }
    }
}