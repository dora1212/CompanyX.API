using CompanyX.API.BusinessLogic.Interfaces;
using CompanyX.API.DataAccess.Entities;
using CompanyX.API.DataAccess.Enums;
using CompanyX.API.DataAccess.Interfaces;
using CompanyX.API.DataAccess.Models;

namespace CompanyX.API.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<Employee> GetEmployeeByIdAsync(Guid employeeId)
        {
            return await _employeeRepository.GetEmployeeById(employeeId);
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByNameAndBirthdateIntervalAsync(string name, DateTime birthdateFrom, DateTime birthDateTo)
        {
            return await _employeeRepository.GetEmployeesByNameAndBirthdateIntervalAsync(name, birthdateFrom, birthDateTo);
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _employeeRepository.GetAllEmployeesAsync();
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByBossIdAsync(Guid bossId)
        {
            return await _employeeRepository.GetEmployeesByBossIdAsync(bossId);
        }

       public async Task<EmployeeStatistic> GetRoleStatisticsAsync(JobTitle role)
        {
            return await _employeeRepository.GetRoleStatisticsAsync(role);
        }

        public async Task AddNewEmployeeAsync(Employee employee)
        {
            await _employeeRepository.AddEmployeeAsync(employee);
        }

        public async Task UpdateEmployeeAsync(Employee newEmployee)
        {
            Employee existingEmployee = await GetEmployeeByIdAsync(newEmployee.Id);
            await _employeeRepository.UpdateEmployeeAsync(existingEmployee, newEmployee);
        }

        public async Task DeleteEmployeeAsync(Guid employeeId)
        {
            await _employeeRepository.DeleteEmployeeAsync(employeeId);
        }

        public async Task<Employee> CreateNewEmployeeAsync(EmployeeData employeeData)
        {
            return await _employeeRepository.CreateNewEmployeeAsync(employeeData);
        }

        public Task UpdateEmployeeSalary(Guid employeeId, decimal salary)
        {
            return _employeeRepository.UpdateSalary(employeeId, salary);
        }
    }
}
