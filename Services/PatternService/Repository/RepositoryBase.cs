using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using northwindAPI.Models;
using northwindAPI.PatternService.UnitOfWork;
using northwindAPI.RepositoryService;

namespace northwindAPI.PatternService.Repository
{
    public sealed class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly AppDbContext _context;
  
        public RepositoryBase(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(TEntity entity)
        {
          var record =  entity;
          await _context.Set<TEntity>().AddAsync(record);
        }

        public async Task DeleteAsync(int id)
        {
            var record = await GetById(id);
            _context.Set<TEntity>().Remove(record);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            var records = await _context.Set<TEntity>().ToListAsync();
            return records;
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var record =  await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
            return record;
        }

        public async Task<TEntity> GetById(int id)
        {
            var record = await _context.Set<TEntity>().FindAsync(id);
            return record;
        }

        public async Task UpdateAsync(int id, TEntity entity)
        {
            var record = await GetById(id);
            _context.Entry(record).CurrentValues.SetValues(entity);
        }

        public Task UpdatePatchAsync(int id, [FromBody] JsonPatchDocument<TEntity> patch)
        {
            throw new NotImplementedException();
        }
    }
}