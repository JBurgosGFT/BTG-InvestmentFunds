using Moq;
using InvestmentFunds.Application.Interfaces;
using InvestmentFunds.Application.Features.Customer;
using InvestmentFunds.Application.Dtos;
using InvestmentFunds.Domain.Entities;
using InvestmentFunds.Domain.Enums;


public class GetCustomerQueryHandlerTests
{
    private readonly Mock<ICustomerRepository> _repoMock;
    private readonly GetCustomerQueryHandler _handler;

    public GetCustomerQueryHandlerTests()
    {
        _repoMock = new Mock<ICustomerRepository>();
        _handler = new GetCustomerQueryHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsCustomerDto_WhenCustomerExists()
    {
        var customerId = Guid.NewGuid();
        var customer = new CustomerDto { CustomerId = customerId, Name = "Test", Balance = 100 };
        _repoMock.Setup(r => r.GetCustomerAsync(customerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Customer { CustomerId = customerId, Name = "Test", Balance = 100 });

        var result = await _handler.Handle(new GetCustomerQuery { CustomerId = customerId }, CancellationToken.None);

        Assert.Equal(customerId, result.CustomerId);
        Assert.Equal("Test", result.Name);
        Assert.Equal(100, result.Balance);
    }

    [Fact]
    public async Task Handle_ThrowsException_WhenCustomerDoesNotExist()
    {
        var customerId = Guid.NewGuid();
        _repoMock.Setup(r => r.GetCustomerAsync(customerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Customer)null);

        await Assert.ThrowsAsync<Exception>(() =>
            _handler.Handle(new GetCustomerQuery { CustomerId = customerId }, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ReturnsCorrectEmail()
    {
        var customerId = Guid.NewGuid();
        _repoMock.Setup(r => r.GetCustomerAsync(customerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Customer { CustomerId = customerId, Name = "Test", Email = "test@mail.com" });

        var result = await _handler.Handle(new GetCustomerQuery { CustomerId = customerId }, CancellationToken.None);

        Assert.Equal("test@mail.com", result.Email);
    }

    [Fact]
    public async Task Handle_ReturnsCorrectNotificationPreference()
    {
        var customerId = Guid.NewGuid();
        _repoMock.Setup(r => r.GetCustomerAsync(customerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Customer { CustomerId = customerId, Name = "Test", NotificationPreference = NotificationPreference.Email });

        var result = await _handler.Handle(new GetCustomerQuery { CustomerId = customerId }, CancellationToken.None);

        Assert.Equal(NotificationPreference.Email, result.NotificationPreference);
    }

    [Fact]
    public async Task Handle_CallsRepositoryOnce()
    {
        var customerId = Guid.NewGuid();
        _repoMock.Setup(r => r.GetCustomerAsync(customerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Customer { CustomerId = customerId, Name = "Test" });

        await _handler.Handle(new GetCustomerQuery { CustomerId = customerId }, CancellationToken.None);

        _repoMock.Verify(r => r.GetCustomerAsync(customerId, It.IsAny<CancellationToken>()), Times.Once);
    }
}
