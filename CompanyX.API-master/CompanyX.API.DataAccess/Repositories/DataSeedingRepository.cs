using CompanyX.API.DataAccess.DatabaseContext;
using CompanyX.API.DataAccess.Entities;
using CompanyX.API.DataAccess.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace CompanyX.API.DataAccess.Repositories
{
    public class DataSeedingRepository : IDataSeedingRepository
    {
        protected readonly ILogger<IEmployeeRepository> _logger;
        protected readonly CompanyXDbContext _companyXDbContext;
        private readonly IEmployeeRepository _employeeRepository;

        public DataSeedingRepository(CompanyXDbContext companyXDbContext, ILogger<IEmployeeRepository> logger, IEmployeeRepository employeeRepository)
        {
            _logger = logger;
            _companyXDbContext = companyXDbContext;
            _employeeRepository = employeeRepository;
        }

        private async Task ClearDatabaseAsync()
        {
            _companyXDbContext.Employees.RemoveRange(_companyXDbContext.Employees);
            _companyXDbContext.EmployeeAddresses.RemoveRange(_companyXDbContext.EmployeeAddresses);
            _companyXDbContext.Roles.RemoveRange(_companyXDbContext.Roles);
            await SaveChangesAsync();
        }

        public async Task SeedInitialDataAsync(List<Employee> employees, List<Role> roles)
        {
            try
            {
                await ClearDatabaseAsync();
                await _employeeRepository.AddEmployees(employees);
                await SaveChangesAsync();
            }
            catch(ValidationException ex)
            {
                _logger.LogError($"Database seeding failed. Validation exception occurred: {ex.Message}");
            }
            catch(Exception ex)
            {
                _logger.LogError($"Database seeding failed. Error occurred: {ex.Message}");
            }
        }

        private async Task SaveChangesAsync()
        {
            await _companyXDbContext.SaveChangesAsync();
        }
    }
}
