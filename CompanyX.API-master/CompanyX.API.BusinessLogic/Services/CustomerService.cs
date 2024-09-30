using CompanyX.API.BusinessLogic.Interfaces;
using CompanyX.API.DataAccess.Entities;
using CompanyX.API.DataAccess.Models;
using AutoMapper;
using CompanyX.API.DataAccess.Interfaces;
using Microsoft.Extensions.Logging;

namespace CompanyX.API.BusinessLogic.Services
{
    public class CustomerService : ICustomerService
    {

        private readonly IMapper _mapper;
        private readonly ICustomerRepository _customerRepository;
        private readonly IConcurrencyService _concurrencyService;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(IMapper mapper, ICustomerRepository customerRepository, IConcurrencyService concurrencyService, ILogger<CustomerService> logger)
        {
            _mapper = mapper;
            _customerRepository = customerRepository;
            _concurrencyService = concurrencyService;
            _logger = logger;
        }

        /// <summary>
        /// If an existing customer with given
        /// first name, last name, address, e-mail and phone number
        /// is found, only the credit card field is updated.
        /// If no existing customer is found, a new one is created.
        /// </summary>
        /// <param name="customerDto">Customer to be searched for in db</param>
        /// <returns>updated/newly created record</returns>
        public async Task<Customer> AddOrMergeCustomerAsync(CreateCustomerDto customerDto)
        {
            await _concurrencyService.GetSemaphore().WaitAsync();
            try
            {
                var customer = _mapper.Map<Customer>(customerDto);

                var existingCustomer = await _customerRepository.FindCustomerAsync(customer);

                if (existingCustomer != null)
                {
                    _logger.LogInformation("Existing customer found, merging...");
                    return await _customerRepository.MergeCustomerAsync(existingCustomer, customer);
                }
                _logger.LogInformation("No existing customer found, creating new customer");
                return await _customerRepository.AddCustomerAsync(customer);
            }
            finally
            {
                _concurrencyService.GetSemaphore().Release();
            }
        }

        public async Task<IEnumerable<MergedCustomerHistory>> GetMergedCustomersReportAsync()
        {
            return await _customerRepository.GetMergedCustomersReportAsync();
        }
    }
}
