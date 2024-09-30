using Moq;
using Microsoft.Extensions.Logging;
using AutoMapper;
using CompanyX.API.BusinessLogic.Interfaces;
using CompanyX.API.BusinessLogic.Services;
using CompanyX.API.DataAccess.Entities;
using CompanyX.API.DataAccess.Interfaces;
using CompanyX.API.DataAccess.Models;

public class CustomerServiceTests
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly Mock<IConcurrencyService> _concurrencyServiceMock;
    private readonly Mock<ILogger<CustomerService>> _loggerMock;
    private readonly CustomerService _customerService;

    public CustomerServiceTests()
    {
        _mapperMock = new Mock<IMapper>();
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _concurrencyServiceMock = new Mock<IConcurrencyService>();
        _loggerMock = new Mock<ILogger<CustomerService>>();

        var semaphoreMock = new Mock<SemaphoreSlim>(1, 1);
        _concurrencyServiceMock.Setup(cs => cs.GetSemaphore()).Returns(semaphoreMock.Object);

        _customerService = new CustomerService(
            _mapperMock.Object,
            _customerRepositoryMock.Object,
            _concurrencyServiceMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task AddOrMergeCustomerAsync_ExistingCustomer_FindsAndMergesCustomer()
    {
        // Arrange
        var customerDto = new CreateCustomerDto();
        var customer = new Customer();
        var existingCustomer = new Customer();

        _mapperMock.Setup(m => m.Map<Customer>(customerDto)).Returns(customer);
        _customerRepositoryMock.Setup(cr => cr.FindCustomerAsync(customer)).ReturnsAsync(existingCustomer);
        _customerRepositoryMock.Setup(cr => cr.MergeCustomerAsync(existingCustomer, customer)).ReturnsAsync(existingCustomer);

        // Act
        var result = await _customerService.AddOrMergeCustomerAsync(customerDto);

        // Assert
        Assert.Equal(existingCustomer, result);
        _customerRepositoryMock.Verify(cr => cr.FindCustomerAsync(customer), Times.Once);
        _customerRepositoryMock.Verify(cr => cr.MergeCustomerAsync(existingCustomer, customer), Times.Once);
        _customerRepositoryMock.Verify(cr => cr.AddCustomerAsync(It.IsAny<Customer>()), Times.Never);
    }

    [Fact]
    public async Task AddOrMergeCustomerAsync_NoExistingCustomer_AddsNewCustomer()
    {
        // Arrange
        var customerDto = new CreateCustomerDto();
        var customer = new Customer();

        _mapperMock.Setup(m => m.Map<Customer>(customerDto)).Returns(customer);
        _customerRepositoryMock.Setup(cr => cr.FindCustomerAsync(customer)).ReturnsAsync((Customer)null);
        _customerRepositoryMock.Setup(cr => cr.AddCustomerAsync(customer)).ReturnsAsync(customer);

        // Act
        var result = await _customerService.AddOrMergeCustomerAsync(customerDto);

        // Assert
        Assert.Equal(customer, result);
        _customerRepositoryMock.Verify(cr => cr.FindCustomerAsync(customer), Times.Once);
        _customerRepositoryMock.Verify(cr => cr.AddCustomerAsync(customer), Times.Once);
        _customerRepositoryMock.Verify(cr => cr.MergeCustomerAsync(It.IsAny<Customer>(), It.IsAny<Customer>()), Times.Never);
    }
}