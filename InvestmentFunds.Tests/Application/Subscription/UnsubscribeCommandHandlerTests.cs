using Moq;
using InvestmentFunds.Application.Features.Subscription;
using InvestmentFunds.Application.Interfaces;
using InvestmentFunds.Domain.Entities;
using InvestmentFunds.Domain.Enums;
using MediatR;
using InvestmentFunds.Application.Features.Customer;
using InvestmentFunds.Application.Features.Fund;
using InvestmentFunds.Application.Features.Transaction;
using InvestmentFunds.Application.Features.Notification;
using InvestmentFunds.Application.Dtos;

public class UnsubscribeCommandHandlerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<ISubscriptionRepository> _repoMock;
    private readonly UnsubscribeCommandHandler _handler;

    public UnsubscribeCommandHandlerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _repoMock = new Mock<ISubscriptionRepository>();
        _handler = new UnsubscribeCommandHandler(_mediatorMock.Object, _repoMock.Object);
    }

    [Fact]
    public async Task Handle_ThrowsKeyNotFound_WhenSubscriptionNotFound()
    {
        var cmd = new UnsubscribeCommand { SubscriptionId = Guid.NewGuid(), CustomerId = Guid.NewGuid() };
        _repoMock.Setup(r => r.GetSubscriptionAsync(cmd.SubscriptionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Subscription)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _handler.Handle(cmd, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ReturnsSubscriptionId_WhenAlreadyInactive()
    {
        var cmd = new UnsubscribeCommand { SubscriptionId = Guid.NewGuid(), CustomerId = Guid.NewGuid() };
        var subscription = new Subscription { SubscriptionId = cmd.SubscriptionId, IsActive = false };
        _repoMock.Setup(r => r.GetSubscriptionAsync(cmd.SubscriptionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(subscription);

        var result = await _handler.Handle(cmd, CancellationToken.None);

        Assert.Equal(cmd.SubscriptionId, result);
    }

    [Fact]
    public async Task Handle_ThrowsInvalidOperation_WhenFundIdIsZero()
    {
        var cmd = new UnsubscribeCommand { SubscriptionId = Guid.NewGuid(), CustomerId = Guid.NewGuid() };
        var subscription = new Subscription { SubscriptionId = cmd.SubscriptionId, IsActive = true, FundId = 0 };
        _repoMock.Setup(r => r.GetSubscriptionAsync(cmd.SubscriptionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(subscription);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(cmd, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ThrowsKeyNotFound_WhenCustomerNotFound()
    {
        var cmd = new UnsubscribeCommand { SubscriptionId = Guid.NewGuid(), CustomerId = Guid.NewGuid() };
        var subscription = new Subscription { SubscriptionId = cmd.SubscriptionId, IsActive = true, FundId = 1, Amount = 100 };
        _repoMock.Setup(r => r.GetSubscriptionAsync(cmd.SubscriptionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(subscription);

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetCustomerQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((CustomerDto)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _handler.Handle(cmd, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_UpdatesSubscriptionAndSendsCommands_WhenValid()
    {
        var cmd = new UnsubscribeCommand { SubscriptionId = Guid.NewGuid(), CustomerId = Guid.NewGuid() };
        var subscription = new Subscription { SubscriptionId = cmd.SubscriptionId, IsActive = true, FundId = 1, Amount = 100 };
        var customer = new InvestmentFunds.Application.Dtos.CustomerDto { CustomerId = cmd.CustomerId, Balance = 500, NotificationPreference = NotificationPreference.Email, Email = "mail@mail.com", Phone = "123" };
        var fund = new InvestmentFunds.Application.Dtos.FundDto { FundId = 1, Name = "Fondo X", MinAmount = 1000 };

        _repoMock.Setup(r => r.GetSubscriptionAsync(cmd.SubscriptionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(subscription);

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetCustomerQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(customer);

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetFundQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(fund);

        _repoMock.Setup(r => r.UpdateSubscriptionAsync(subscription, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateTransactionCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Guid.NewGuid());

        _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateCustomerBalanceCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(customer);

        _mediatorMock.Setup(m => m.Send(It.IsAny<SendNotificationCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        var result = await _handler.Handle(cmd, CancellationToken.None);

        Assert.Equal(cmd.SubscriptionId, result);
        _repoMock.Verify(r => r.UpdateSubscriptionAsync(subscription, It.IsAny<CancellationToken>()), Times.Once);
        _mediatorMock.Verify(m => m.Send(It.IsAny<CreateTransactionCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        _mediatorMock.Verify(m => m.Send(It.IsAny<UpdateCustomerBalanceCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        _mediatorMock.Verify(m => m.Send(It.IsAny<SendNotificationCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}