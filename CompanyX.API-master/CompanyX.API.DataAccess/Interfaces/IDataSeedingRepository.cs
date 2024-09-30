using CompanyX.API.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyX.API.DataAccess.Interfaces
{
    public interface IDataSeedingRepository
    {
        Task SeedInitialDataAsync(List<Employee> employees, List<Role> roles);
    }
}
