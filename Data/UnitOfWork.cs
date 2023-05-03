using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.core.IConfigurations;
using WebApi.core.iRepositories;
using WebApi.core.Repositories;

namespace WebApi.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly EmployeeContext _context;
        private readonly ILogger _logger;
        public IEmployeeRepository Employees {get; private set;}

        public UnitOfWork(
            EmployeeContext context,
            ILoggerFactory loggerFactory
        )
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("logs");

            Employees = new EmployeeRepository(_context, _logger);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();   
        }
    }
}