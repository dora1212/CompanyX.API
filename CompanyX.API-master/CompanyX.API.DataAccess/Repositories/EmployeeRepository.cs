using CompanyX.API.DataAccess.DatabaseContext;
using CompanyX.API.DataAccess.Entities;
using CompanyX.API.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FluentValidation;
using ValidationException = FluentValidation.ValidationException;
using CompanyX.API.DataAccess.Enums;
using CompanyX.API.DataAccess.Models;
using System;

namespace CompanyX.API.DataAccess.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        protected readonly CompanyXDbContext _companyXDbContext;
        protected readonly ILogger<IEmployeeRepository> _logger;
        private readonly IValidator<Employee> _employeeValidator;

        public EmployeeRepository(CompanyXDbContext companyXDbContext, ILogger<IEmployeeRepository> logger, IValidator<Employee> employeeValidator)
        {
            _logger = logger;
            _companyXDbContext = companyXDbContext;
            _employeeValidator = employeeValidator;
        }

        public async  Task<Employee> GetEmployeeById(Guid id)
        {
 
            var employee = await _companyXDbContext.Employees.FirstOrDefaultAsync(employee => employee.Id == id);

            if (employee == null)
            {
                throw new FileNotFoundException($"Employee with an id {id} was not found.");
            }

            return employee;
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByNameAndBirthdateIntervalAsync(string name, DateTime dateOfBirthFrom, DateTime dateOfBirthTo)
        {
            var employees = await _companyXDbContext.Employees.Where(x => x.FirstName == name && x.BirthDate > dateOfBirthFrom && x.BirthDate < dateOfBirthTo).ToListAsync();

            if (employees.Count == 0)
            {
                throw new FileNotFoundException($"No employees with name {name} and birthdate interval from {dateOfBirthFrom} to {dateOfBirthTo} were found.");
            }

            return employees;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            var allEmployees = await _companyXDbContext.Employees.ToListAsync();

            return allEmployees;
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByBossIdAsync(Guid id)
        {
            var bossExists = _companyXDbContext.Employees.Any(x => x.Role.Name == JobTitle.Ceo && x.Id == id);

            if (!bossExists)
            {
                throw new FileNotFoundException($"Boss with a given id {id} does not exist.");
            }

            return await _companyXDbContext.Employees.Where(x => x.Boss.Id == id).ToListAsync();
        }

        public async Task<EmployeeStatistic> GetRoleStatisticsAsync(JobTitle role)
        {
            EmployeeStatistic employeeStatistic = new EmployeeStatistic();
            employeeStatistic.JobTitle = role.ToString();
            employeeStatistic.EmployeeCount = await _companyXDbContext.Employees.Where(x => x.Role.Name == role).CountAsync();
            employeeStatistic.AverageWage = _companyXDbContext.Employees.Where(x => x.Role.Name == role).Select(x => x.CurentSalary).ToList().Sum() / employeeStatistic.EmployeeCount;
            return employeeStatistic;
        }

        public async Task AddEmployeeAsync(Employee employee)
        {
            await ValidateEmployee(employee);
            await _companyXDbContext.Employees.AddAsync(employee);
            await SaveChangesAsync();
        }

        public async Task DeleteEmployeeAsync(Guid employeeId)
        {
            var employee = await GetEmployeeById(employeeId);
            _companyXDbContext.Employees.Remove(employee);
            await SaveChangesAsync();
        }

        public async Task AddEmployees(List<Employee> employees)
        {
            await ValidateEmployees(employees);
            await _companyXDbContext.Employees.AddRangeAsync(employees);
            await SaveChangesAsync();
        }

        public async Task UpdateSalary(Guid employeeId, decimal salary)
        {
            Employee employee = await _companyXDbContext.Employees.Include(x => x.HomeAddress).Include(x => x.Role).Include(x => x.Boss).FirstAsync(x => x.Id == employeeId);
            employee.CurentSalary = salary;
            await RunFluentValidation(employee);
            _companyXDbContext.Employees.Update(employee);
            await SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _companyXDbContext.SaveChangesAsync();
        }

        public async Task<Employee> CreateNewEmployeeAsync(EmployeeData employeeData)
        {
            EmployeeHomeAddress homeAddress = new EmployeeHomeAddress
            {
                StreetName = employeeData.StreetName,
                HouseNumber = employeeData.HouseNumber,
                City = employeeData.City,
                PostalCode = employeeData.PostalCode
            };

            Employee employee = new Employee
            {
                FirstName = employeeData.Name,
                LastName = employeeData.LastName,
                BirthDate = employeeData.BirthDate,
                EmploymentDate = employeeData.EmploymentDate,
                CurentSalary = employeeData.CurrentSalary,
                HomeAddress = homeAddress,
                Role = await _companyXDbContext.Roles.FirstAsync(x => x.Id == employeeData.RoleId),
                Boss = await _companyXDbContext.Employees.FirstAsync(x => x.Id == employeeData.BossId)
            };

            return employee;
        }

        public async Task UpdateEmployeeAsync(Employee existingEmployee, Employee newEmployee)
        {
            await RunFluentValidation(newEmployee);
            existingEmployee = newEmployee;
            _companyXDbContext.Employees.Update(existingEmployee);
            await SaveChangesAsync();
        }

        public async Task<Guid> GetRoleIdByName(JobTitle role)
        {
            if (!_companyXDbContext.Roles.Where(x => x.Name == role).Any())
            {
                _logger.LogError("Role could not be found");
            }
            return _companyXDbContext.Roles.FirstOrDefault(x => x.Name == role).Id;
        }

        private async Task ValidateEmployees(List<Employee> employees)
        {
            if (employees.Where(x => x.Role.Name == JobTitle.Ceo).Count() > 1)
            {
                throw new ValidationException("Only one person with CEO role per database allowed");
            }

            if (employees.Any(x => x.Role.Name == JobTitle.Ceo))
            {
                if (await _companyXDbContext.Employees.AnyAsync() && await _companyXDbContext.Employees.AnyAsync(x => x.Role.Name == JobTitle.Ceo))
                {
                    throw new ValidationException("Only one person with CEO role per database allowed");
                }
            }

            foreach (var employee in employees)
            {
                await RunFluentValidation(employee);
            }
        }

        private async Task ValidateEmployee(Employee employee)
        {
            if (employee.Role.Name == JobTitle.Ceo)
            {
                if (await _companyXDbContext.Employees.AnyAsync() && await _companyXDbContext.Employees.AnyAsync(x => x.Role.Name == JobTitle.Ceo))
                {
                    throw new ValidationException("Only one person with CEO role per database allowed");
                }
            }

            await RunFluentValidation(employee);
        }

        private async Task RunFluentValidation(Employee employee)
        {
            var result = await _employeeValidator.ValidateAsync(employee);
            if (!result.IsValid)
            {
                FluentValidation.Results.ValidationResult validationResult = result;
                throw new ValidationException(result.Errors[0].ErrorMessage);
            }
        }



    }
}
