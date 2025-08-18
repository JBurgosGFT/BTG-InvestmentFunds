using Moq;
using InvestmentFunds.Application.Features.Notification;
using InvestmentFunds.Application.Notifications;
using InvestmentFunds.Domain.Enums;

public class SendNotificationCommandHandlerTests
{
    private readonly Mock<INotificationStrategy> _notificationStrategyMock;
    private readonly Mock<INotificationStrategy> _emailMock;
    private readonly Mock<INotificationStrategy> _smsMock;
    private readonly SendNotificationCommandHandler _handler;

    public SendNotificationCommandHandlerTests()
    {
        _notificationStrategyMock = new Mock<INotificationStrategy>();
        _emailMock = new Mock<INotificationStrategy>();
        _smsMock = new Mock<INotificationStrategy>();
        _handler = new SendNotificationCommandHandler(
            _notificationStrategyMock.Object,
            _emailMock.Object,
            _smsMock.Object
        );
    }

    [Fact]
    public async Task Handle_SendsEmail_WhenPreferenceIsEmail()
    {
        var cmd = new SendNotificationCommand
        {
            FundName = "Fondo A",
            Amount = "1000",
            NotificationPreference = NotificationPreference.Email,
            Email = "test@mail.com",
            Type = TransactionType.Opening
        };

        await _handler.Handle(cmd, CancellationToken.None);

        _emailMock.Verify(x => x.SendAsync("test@mail.com", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        _smsMock.Verify(x => x.SendAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_SendsSms_WhenPreferenceIsSms_AndPhoneProvided()
    {
        var cmd = new SendNotificationCommand
        {
            FundName = "Fondo B",
            Amount = "2000",
            NotificationPreference = NotificationPreference.SMS,
            Phone = "123456789",
            Type = TransactionType.Opening
        };

        await _handler.Handle(cmd, CancellationToken.None);

        _smsMock.Verify(x => x.SendAsync("123456789", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        _emailMock.Verify(x => x.SendAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ThrowsException_WhenTransactionTypeIsInvalid()
    {
        var cmd = new SendNotificationCommand
        {
            FundName = "Fondo C",
            Amount = "3000",
            NotificationPreference = NotificationPreference.Email,
            Email = "test@mail.com",
            Type = (TransactionType)999 // Invalid type
        };

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            _handler.Handle(cmd, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_SendsNotificationStrategyAlways()
    {
        var cmd = new SendNotificationCommand
        {
            FundName = "Fondo D",
            Amount = "4000",
            NotificationPreference = NotificationPreference.Email,
            Email = "test@mail.com",
            Type = TransactionType.Cancellation
        };

        await _handler.Handle(cmd, CancellationToken.None);

        _notificationStrategyMock.Verify(x => x.SendAsync("test@mail.com", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_SendsCorrectSubjectAndBody_ForOpening()
    {
        var cmd = new SendNotificationCommand
        {
            FundName = "Fondo E",
            Amount = "5000",
            NotificationPreference = NotificationPreference.Email,
            Email = "test@mail.com",
            Type = TransactionType.Opening
        };

        await _handler.Handle(cmd, CancellationToken.None);

        _emailMock.Verify(x => x.SendAsync(
            "test@mail.com",
            "Suscripción a Fondo E",
            "Se abrió su suscripción por $5000 al fondo Fondo E.",
            It.IsAny<CancellationToken>()),Times.Once);
    }
}