using CompanyX.API.DataAccess.Entities;
using CompanyX.API.DataAccess.Enums;
using CompanyX.API.DataAccess.Models;

namespace CompanyX.API.DataAccess.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<Guid> GetRoleIdByName(JobTitle role);
        Task<Employee> GetEmployeeById(Guid id);
        Task<IEnumerable<Employee>> GetEmployeesByNameAndBirthdateIntervalAsync(string name, DateTime dateOfBirthFrom, DateTime dateOfBirthTo);
        Task<IEnumerable<Employee>> GetEmployeesByBossIdAsync(Guid id);
        Task<EmployeeStatistic> GetRoleStatisticsAsync(JobTitle role);
        Task UpdateSalary(Guid employeeId, decimal salary);
        Task DeleteEmployeeAsync(Guid employeeId);
        Task SaveChangesAsync();
        Task AddEmployees(List<Employee> employees);
        Task AddEmployeeAsync(Employee employee);
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<Employee> CreateNewEmployeeAsync(EmployeeData dto);
        Task UpdateEmployeeAsync(Employee existingEmployee, Employee newEmployee);
    }
}
