using CompanyX.API.DataAccess.Entities;
using CompanyX.API.DataAccess.Models;

namespace CompanyX.API.BusinessLogic.Interfaces
{
    public interface ICustomerService
    {
        Task<Customer> AddOrMergeCustomerAsync(CreateCustomerDto customerDto);
        Task<IEnumerable<MergedCustomerHistory>> GetMergedCustomersReportAsync();
    }
}
