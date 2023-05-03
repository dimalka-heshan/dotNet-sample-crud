using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.core.iRepositories;

namespace WebApi.core.IConfigurations
{
    public interface IUnitOfWork
    {
        IEmployeeRepository Employees {get;}

        Task CompleteAsync();
    }
}