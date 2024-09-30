using CompanyX.API.DataAccess.DatabaseContext;
using CompanyX.API.DataAccess.Entities;
using CompanyX.API.DataAccess.Interfaces;
using CompanyX.API.DataAccess.Utilities;
using Microsoft.EntityFrameworkCore;

namespace CompanyX.API.DataAccess.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        protected readonly CompanyXDbContext _companyXDbContext;

        public CustomerRepository(CompanyXDbContext companyXDbContext)
        {
            _companyXDbContext = companyXDbContext;
        }

        /// <summary>
        /// Searches for an existing customer in the database with matching
        /// first name, last name, address, e-mail and phone number.
        /// </summary>
        /// <param name="customer">Customer to be searched for in db</param>
        /// <returns>Customer that was found</returns>
        public async Task<Customer?> FindCustomerAsync(Customer customer)
        {
            var customers = await _companyXDbContext.Customers
                .Include(c => c.HomeAddress)
                .ToListAsync();

            return customers.FirstOrDefault(c => CustomerComparer.AreCustomersEqual(c, customer));
        }

        /// <summary>
        /// Merges old customer object with an incoming customer object. 
        /// Only cardNumber is be updated because all the remaining fields of the entity are used to identify an existing customer and therefore cannot be updated.
        /// </summary>
        /// <param name="existingCustomer"> old customer object</param>
        /// <param name="newCustomer">incoming customer object</param>
        /// <returns>Updated customer object</returns>
        public async Task<Customer> MergeCustomerAsync(Customer existingCustomer, Customer newCustomer)
        {
            if (existingCustomer.CardNumber != newCustomer.CardNumber)
            {
                existingCustomer.CardNumber = newCustomer.CardNumber;
                existingCustomer.IsUpdated = true;
                await AddMergedCustomerHistoryAsync(existingCustomer.Id);
                await SaveChangesAsync();
            }
            return existingCustomer;
        }

        public async Task<Customer> AddCustomerAsync(Customer customer)
        {
            await _companyXDbContext.Customers.AddAsync(customer);
            await SaveChangesAsync();
            return customer;
        }

        public async Task<IEnumerable<MergedCustomerHistory>> GetMergedCustomersReportAsync()
        {
            return await _companyXDbContext.MergedCustomerHistories
                .Include(mch => mch.Customer)
                .ToListAsync();
        }

        private async Task AddMergedCustomerHistoryAsync(Guid existingCustomerId)
        {
            var mergedCustomerHistory = new MergedCustomerHistory
            {
                CustomerId = existingCustomerId,
                MergeDate = DateTime.UtcNow
            };

            await _companyXDbContext.MergedCustomerHistories.AddAsync(mergedCustomerHistory);
        }

        private async Task SaveChangesAsync()
        {
            await _companyXDbContext.SaveChangesAsync();
        }
    }
}
