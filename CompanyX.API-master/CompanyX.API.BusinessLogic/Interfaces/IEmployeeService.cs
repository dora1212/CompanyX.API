using CompanyX.API.DataAccess.Entities;
using CompanyX.API.DataAccess.Enums;
using CompanyX.API.DataAccess.Models;

namespace CompanyX.API.BusinessLogic.Interfaces
{
    public interface IEmployeeService
    {
        Task<Employee> GetEmployeeByIdAsync(Guid employeeId);
        Task<IEnumerable<Employee>> GetEmployeesByNameAndBirthdateIntervalAsync(string name, DateTime birthdateFrom, DateTime birthDateTo);
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<IEnumerable<Employee>> GetEmployeesByBossIdAsync(Guid bossId);
        Task<EmployeeStatistic> GetRoleStatisticsAsync(JobTitle role);
        Task AddNewEmployeeAsync(Employee employee);
        Task UpdateEmployeeAsync(Employee employee);
        Task UpdateEmployeeSalary(Guid employeeId, decimal salary);
        Task DeleteEmployeeAsync(Guid employeeId);
        Task<Employee> CreateNewEmployeeAsync(EmployeeData employeeData);
    }
}
