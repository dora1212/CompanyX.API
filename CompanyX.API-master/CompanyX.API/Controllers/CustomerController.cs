using CompanyX.API.BusinessLogic.Interfaces;
using CompanyX.API.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CompanyX.API.Controllers
{
    public class CustomerController : ControllerBase
    {
        protected readonly ILogger<CustomerController> _logger;
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService, ILogger<CustomerController> logger)
        {
            _logger = logger;
            _customerService = customerService;
        }

        /// <summary>
        /// Adds/merges a customer
        /// </summary>
        /// <response code="201">New customer created</response>
        /// <response code="200">Old customer updated/merged</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Server side error</response>
        [HttpPost]
        [Route("Customer")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Customer([FromBody] CreateCustomerDto customerDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var customer = await _customerService.AddOrMergeCustomerAsync(customerDto);

                if (customer.IsUpdated)
                {
                    return Ok(new { Message = "Old customer updated/merged.", Customer = customer });
                }
                else
                {
                    return StatusCode(StatusCodes.Status201Created, new { Message = "New customer was created.", Customer = customer });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server side error occurred: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Gets report of all customers that have history of being merged.
        /// </summary>
        /// <returns>A list of customer that have been merged</returns>
        [HttpGet]
        [Route("MergedCustomersReport")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> MergedCustomersReport()
        {
            try
            {
                var mergedCustomers = await _customerService.GetMergedCustomersReportAsync();
                return Ok(mergedCustomers);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server side error occurred: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
