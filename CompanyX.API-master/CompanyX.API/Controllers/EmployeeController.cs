using CompanyX.API.BusinessLogic.Interfaces;
using CompanyX.API.DataAccess.Entities;
using CompanyX.API.DataAccess.Enums;
using CompanyX.API.DataAccess.Interfaces;
using CompanyX.API.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ValidationException = FluentValidation.ValidationException;

namespace CompanyX.API.Controllers
{
    [Route("api/")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        protected readonly ILogger<IEmployeeRepository> _logger;

        public EmployeeController(IEmployeeService employeeService, ILogger<IEmployeeRepository> logger)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        /// <summary>
        /// Gets an employee by id
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Server side error</response>
        [HttpGet]
        [Route("Employee/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Employee(Guid id)
        {
            try
            {
                var employee = await _employeeService.GetEmployeeByIdAsync(id);
                return Ok(employee);
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError($"Client side error occurred: {ex.Message}");
                return BadRequest($"Bad Request: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server side error occurred: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Gets employees by name and birthdate interval
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Server side error</response>
        [HttpGet]
        [Route("Employees/{firstName}/{birthdateFrom}/{birthdateTo}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Employees(string firstName, DateTime birthdateFrom, DateTime birthdateTo)
        {
            try
            {
                var employees = await _employeeService.GetEmployeesByNameAndBirthdateIntervalAsync(firstName, birthdateFrom, birthdateTo);
                return Ok(employees);
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError($"Client side error occurred: {ex.Message}");
                return BadRequest($"Bad Request: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server side error occurred: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Gets all employees
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="500">Server side error</response>
        [HttpGet]
        [Route("Employees")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Employees()
        {
            try
            {
                var employees = await _employeeService.GetAllEmployeesAsync();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server side error occurred: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Gets employees by boss id
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Server side error</response>
        [HttpGet]
        [Route("Employees/Boss/{bossId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> EmployeesByBossId(Guid bossId)
        {
            try
            {
                var employees = await _employeeService.GetEmployeesByBossIdAsync(bossId);
                return Ok(employees);
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError($"Client side error occurred: {ex.Message}");
                return BadRequest($"Bad Request: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server side error occurred: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Gets employee count and average salary for particular role
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="500">Server side error</response>
        [HttpGet]
        [Route("Employee/Role/{role}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Role(JobTitle role)
        {
            try
            {
                var statistic = await _employeeService.GetRoleStatisticsAsync(role);
                return Ok(statistic);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server side error occurred: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Adds a new employee
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Server side error</response>
        [HttpPost]
        [Route("Employee")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Employee(EmployeeData employeeData)
        {
            try 
            {
                Employee employee = await _employeeService.CreateNewEmployeeAsync(employeeData);
                await _employeeService.AddNewEmployeeAsync(employee);
                return Ok();
            }
            catch (ValidationException ex)
            {
                _logger.LogError($"Client side error occurred: {ex.Message}");
                return BadRequest($"Bad Request: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server side error occurred: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Updates existing employee
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Server side error</response>
        [HttpPut]
        [Route("Employee")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UpdateEmployee(Employee employee)
        {
            try
            {
                await _employeeService.UpdateEmployeeAsync(employee);
                return Ok();
            }
            catch (ValidationException ex)
            {
                _logger.LogError($"Client side error occurred: {ex.Message}");
                return BadRequest($"Bad Request: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server side error occurred: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        /// <summary>
        /// Updates just employee salary
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Server side error</response>
        [HttpPut]
        [Route("Employee/Salary")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Salary(Guid employeeId, decimal newSalary)
        {
            try
            {
                await _employeeService.UpdateEmployeeSalary(employeeId, newSalary);
                return Ok();
            }
            catch (ValidationException ex)
            {
                _logger.LogError($"Client side error occurred: {ex.Message}");
                return BadRequest($"Bad Request: {ex.Message}");
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError($"Client side error occurred: {ex.Message}");
                return BadRequest($"Bad Request: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server side error occurred: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Deletes an employee
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Server side error</response>
        [HttpDelete]
        [Route("Employee")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Delete(Guid employeeId)
        {
            try
            {
                await _employeeService.DeleteEmployeeAsync(employeeId);
                return Ok();
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError($"Client side error occurred: {ex.Message}");
                return BadRequest($"Bad Request: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server side error occurred: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
