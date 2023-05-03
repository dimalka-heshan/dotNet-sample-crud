using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.core.iRepositories;
using WebApi.Data;

namespace WebApi.core.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected EmployeeContext? _context;
        protected DbSet<T> dbSet;

        protected readonly ILogger _logger;

        public GenericRepository(
            EmployeeContext context,
            ILogger logger
        )
        {
            _context = context;
            _logger = logger;
            this.dbSet = context.Set<T>();
        }

        public  virtual async Task<IEnumerable<T>> All()
        {
            return await dbSet.ToListAsync();
        }

        public virtual async Task<T> GetById(Guid id)
        {
            return await dbSet.FindAsync(id);
        }

        public virtual async Task<bool> Add(T entity)
        {
            await dbSet.AddAsync(entity);
            return true;
        }

        public virtual Task<bool> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public  virtual Task<bool> Upsert(T entity)
        {
            throw new NotImplementedException();
        }
    }
}