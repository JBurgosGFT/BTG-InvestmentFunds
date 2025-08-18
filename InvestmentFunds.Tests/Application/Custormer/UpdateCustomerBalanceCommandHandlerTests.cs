using Moq;
using InvestmentFunds.Application.Features.Customer;
using InvestmentFunds.Application.Interfaces;
using InvestmentFunds.Domain.Entities;
using InvestmentFunds.Domain.Enums;

public class UpdateCustomerBalanceCommandHandlerTests
{
    private readonly Mock<ICustomerRepository> _repoMock;
    private readonly UpdateCustomerBalanceCommandHandler _handler;

    public UpdateCustomerBalanceCommandHandlerTests()
    {
        _repoMock = new Mock<ICustomerRepository>();
        _handler = new UpdateCustomerBalanceCommandHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_UpdatesBalance_WhenCustomerExists()
    {
        var customerId = Guid.NewGuid();
        var customer = new Customer { CustomerId = customerId, Name = "Test", Balance = 50 };
        _repoMock.Setup(r => r.GetCustomerAsync(customerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(customer);

        _repoMock.Setup(r => r.CreateOrUpdateCustomerAsync(customer, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _handler.Handle(new UpdateCustomerBalanceCommand { CustomerId = customerId, NewBalance = 200 }, CancellationToken.None);

        Assert.Equal(200, result.Balance);
    }

    [Fact]
    public async Task Handle_ThrowsKeyNotFound_WhenCustomerDoesNotExist()
    {
        var customerId = Guid.NewGuid();
        _repoMock.Setup(r => r.GetCustomerAsync(customerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Customer)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _handler.Handle(new UpdateCustomerBalanceCommand { CustomerId = customerId, NewBalance = 100 }, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_CallsUpdateMethodOnce()
    {
        var customerId = Guid.NewGuid();
        var customer = new Customer { CustomerId = customerId, Name = "Test", Balance = 50 };
        _repoMock.Setup(r => r.GetCustomerAsync(customerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(customer);

        _repoMock.Setup(r => r.CreateOrUpdateCustomerAsync(customer, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await _handler.Handle(new UpdateCustomerBalanceCommand { CustomerId = customerId, NewBalance = 100 }, CancellationToken.None);

        _repoMock.Verify(r => r.CreateOrUpdateCustomerAsync(customer, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ReturnsCorrectCustomerDto()
    {
        var customerId = Guid.NewGuid();
        var customer = new Customer { CustomerId = customerId, Name = "Test", Balance = 50, Email = "mail@mail.com", NotificationPreference = NotificationPreference.Email };
        _repoMock.Setup(r => r.GetCustomerAsync(customerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(customer);

        _repoMock.Setup(r => r.CreateOrUpdateCustomerAsync(customer, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _handler.Handle(new UpdateCustomerBalanceCommand { CustomerId = customerId, NewBalance = 150 }, CancellationToken.None);

        Assert.Equal(customerId, result.CustomerId);
        Assert.Equal("Test", result.Name);
        Assert.Equal("mail@mail.com", result.Email);
        Assert.Equal(NotificationPreference.Email, result.NotificationPreference);
    }

    [Fact]
    public async Task Handle_UpdatesBalanceProperty()
    {
        var customerId = Guid.NewGuid();
        var customer = new Customer { CustomerId = customerId, Name = "Test", Balance = 50 };
        _repoMock.Setup(r => r.GetCustomerAsync(customerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(customer);

        _repoMock.Setup(r => r.CreateOrUpdateCustomerAsync(customer, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await _handler.Handle(new UpdateCustomerBalanceCommand { CustomerId = customerId, NewBalance = 300 }, CancellationToken.None);

        Assert.Equal(300, customer.Balance);
    }
}
