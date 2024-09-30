using CompanyX.API.DataAccess.Entities;

namespace CompanyX.API.DataAccess.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer?> FindCustomerAsync(Customer customer);
        Task<Customer> AddCustomerAsync(Customer customer);
        Task<Customer> MergeCustomerAsync(Customer existingCustomer, Customer newCustomer);
        Task<IEnumerable<MergedCustomerHistory>> GetMergedCustomersReportAsync();
    }
}
