using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyX.API.BusinessLogic.Interfaces
{
    public interface IDataSeedingService
    {
        Task SeedInitialDataAsync();
    }
}
