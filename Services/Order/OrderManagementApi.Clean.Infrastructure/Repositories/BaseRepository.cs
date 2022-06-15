using Microsoft.EntityFrameworkCore;
using OrderManagementApi.Clean.Application.Contracts.Persistence;
using OrderManagementApi.Clean.Domain.Common;
using OrderManagementApi.Clean.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementApi.Clean.Infrastructure.Repositories
{
    public class BaseRepository<T> : IGenericAsyncRepository<T> where T : EntityBase
    {
        protected readonly OrderContext _dbContext;

        public BaseRepository(OrderContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid customerId)
        {
            return await _dbContext.Set<T>().FindAsync(customerId);
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
