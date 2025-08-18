using Moq;
using InvestmentFunds.Application.Features.Subscription;
using InvestmentFunds.Application.Features.Customer;
using InvestmentFunds.Application.Features.Fund;
using InvestmentFunds.Application.Features.Notification;
using InvestmentFunds.Application.Features.Transaction;
using InvestmentFunds.Application.Interfaces;
using InvestmentFunds.Domain.Entities;
using InvestmentFunds.Domain.Enums;
using InvestmentFunds.Application.Dtos;
using MediatR;

public class SubscribeCommandHandlerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<ISubscriptionRepository> _repoMock;
    private readonly SubscribeCommandHandler _handler;

    public SubscribeCommandHandlerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _repoMock = new Mock<ISubscriptionRepository>();
        _handler = new SubscribeCommandHandler(_mediatorMock.Object, _repoMock.Object);
    }

    [Fact]
    public async Task Handle_ThrowsInvalidOperation_WhenBalanceIsInsufficient()
    {
        var cmd = new SubscribeCommand { FundId = 1, CustomerId = Guid.NewGuid() };
        var customer = new CustomerDto { CustomerId = cmd.CustomerId, Balance = 500, NotificationPreference = NotificationPreference.Email };
        var fund = new FundDto { FundId = 1, Name = "Fondo X", MinAmount = 1000 };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetCustomerQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(customer);
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetFundQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(fund);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(cmd, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_CreatesSubscription_WhenValid()
    {
        var cmd = new SubscribeCommand { FundId = 2, CustomerId = Guid.NewGuid() };
        var customer = new CustomerDto { CustomerId = cmd.CustomerId, Balance = 2000, NotificationPreference = NotificationPreference.Email };
        var fund = new FundDto { FundId = 2, Name = "Fondo Y", MinAmount = 1000 };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetCustomerQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(customer);
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetFundQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(fund);

        _repoMock.Setup(r => r.CreateSubscriptionAsync(It.IsAny<Subscription>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _handler.Handle(cmd, CancellationToken.None);

        Assert.NotEqual(Guid.Empty, result);
        _repoMock.Verify(r => r.CreateSubscriptionAsync(It.IsAny<Subscription>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_CallsCreateTransactionCommand()
    {
        var cmd = new SubscribeCommand { FundId = 3, CustomerId = Guid.NewGuid() };
        var customer = new CustomerDto { CustomerId = cmd.CustomerId, Balance = 1500, NotificationPreference = NotificationPreference.Email };
        var fund = new FundDto { FundId = 3, Name = "Fondo Z", MinAmount = 1000 };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetCustomerQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(customer);
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetFundQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(fund);

        _repoMock.Setup(r => r.CreateSubscriptionAsync(It.IsAny<Subscription>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateTransactionCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Guid.NewGuid());

        await _handler.Handle(cmd, CancellationToken.None);

        _mediatorMock.Verify(m => m.Send(It.IsAny<CreateTransactionCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_CallsUpdateCustomerBalanceCommand()
    {
        var cmd = new SubscribeCommand { FundId = 4, CustomerId = Guid.NewGuid() };
        var customer = new CustomerDto { CustomerId = cmd.CustomerId, Balance = 2000, NotificationPreference = NotificationPreference.Email };
        var fund = new FundDto { FundId = 4, Name = "Fondo W", MinAmount = 1000 };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetCustomerQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(customer);
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetFundQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(fund);

        _repoMock.Setup(r => r.CreateSubscriptionAsync(It.IsAny<Subscription>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await _handler.Handle(cmd, CancellationToken.None);

        _mediatorMock.Verify(m => m.Send(It.IsAny<UpdateCustomerBalanceCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_CallsSendNotificationCommand()
    {
        var cmd = new SubscribeCommand { FundId = 5, CustomerId = Guid.NewGuid() };
        var customer = new CustomerDto { CustomerId = cmd.CustomerId, Balance = 3000, NotificationPreference = NotificationPreference.Email, Email = "mail@mail.com", Phone = "123" };
        var fund = new FundDto { FundId = 5, Name = "Fondo V", MinAmount = 1000 };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetCustomerQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(customer);
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetFundQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(fund);

        _repoMock.Setup(r => r.CreateSubscriptionAsync(It.IsAny<Subscription>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await _handler.Handle(cmd, CancellationToken.None);

        _mediatorMock.Verify(m => m.Send(It.IsAny<SendNotificationCommand>(),It.IsAny<CancellationToken>()), Times.Once);
    }
}