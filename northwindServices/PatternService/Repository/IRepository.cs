using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Azure;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace northwindAPI.RepositoryService
{
    public interface IRepository<TEntity> where TEntity:class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetById(int id);
        Task<TEntity> GetAsync(Expression<Func<TEntity,bool>> predicate);
        Task CreateAsync(TEntity entity);
        Task UpdateAsync(int id,TEntity entity);
        Task DeleteAsync(int id);
        Task UpdatePatchAsync(int id, [FromBody] JsonPatchDocument<TEntity> patch);
        
    }
}