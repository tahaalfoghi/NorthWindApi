using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using northwindAPI.PatternService.Repository;
using northwindAPI.RepositoryService;

namespace northwindAPI.PatternService.UnitOfWork
{
    public interface IUnitOfWork:IDisposable
    {
        CustomerRepository customerRepository {get;}
        OrderDetailsRepository orderDetailsRepository {get;}
        TerritoryRepository territoryRepository {get;}
         new void Dispose();
        Task SaveChangeAsync(CancellationToken token);
    }
}