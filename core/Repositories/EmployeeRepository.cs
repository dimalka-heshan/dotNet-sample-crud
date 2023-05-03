using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.core.iRepositories;
using WebApi.Data;
using Microsoft.EntityFrameworkCore;

namespace WebApi.core.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(
            EmployeeContext context,
            ILogger logger
        ) : base (context, logger)
        {
            
        }

        public override async Task<IEnumerable<Employee>> All()
        {
            try
            {
                return await dbSet.ToListAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "{Repo} All method error",typeof(EmployeeRepository));
                return new List<Employee>();
            }
        }

        public override async Task<bool> Upsert (Employee entity)
        {
             try
            {
                var existingEmployee = await dbSet.Where(x => x.Id == entity.Id)
                                            .FirstOrDefaultAsync();
                
                if (existingEmployee == null)
                    return await Add(entity);

                existingEmployee.Name = entity.Name;
                existingEmployee.Id = entity.Id;
                existingEmployee.City = entity.City;

                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "{Repo} Upsert method error",typeof(EmployeeRepository));
                return false;
            }
        }

        public override async Task<bool> Delete(Guid id)
        {
            try
            {
                var exist = await dbSet.Where(x => x.Id == id)
                                    .FirstOrDefaultAsync();

                if (exist != null)
                {
                    dbSet.Remove(exist);
                    return true;
                }

                return false;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "{Repo} Delete method error",typeof(EmployeeRepository));
                return false;
            }
        }
    }
}