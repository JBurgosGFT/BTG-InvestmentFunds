using Moq;
using InvestmentFunds.Application.Features.Transaction;
using InvestmentFunds.Application.Interfaces;
using InvestmentFunds.Domain.Entities;
using InvestmentFunds.Domain.Enums;

public class CreateTransactionCommandHandlerTests
{
    private readonly Mock<ITransactionRepository> _repoMock;
    private readonly CreateTransactionCommandHandler _handler;

    public CreateTransactionCommandHandlerTests()
    {
        _repoMock = new Mock<ITransactionRepository>();
        _handler = new CreateTransactionCommandHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_CreatesTransaction_ReturnsTransactionId()
    {
        var cmd = new CreateTransactionCommand
        {
            CustomerId = Guid.NewGuid(),
            FundId = 1,
            Amount = 1000,
            Type = TransactionType.Opening
        };
        var expectedId = Guid.NewGuid();

        _repoMock.Setup(r => r.CreateTransactionAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedId);

        var result = await _handler.Handle(cmd, CancellationToken.None);

        Assert.Equal(expectedId, result);
        _repoMock.Verify(r => r.CreateTransactionAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_SetsCorrectTransactionProperties()
    {
        var cmd = new CreateTransactionCommand
        {
            CustomerId = Guid.NewGuid(),
            FundId = 2,
            Amount = 500,
            Type = TransactionType.Cancellation
        };

        Transaction captured = null;
        _repoMock.Setup(r => r.CreateTransactionAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()))
            .Callback<Transaction, CancellationToken>((t, _) => captured = t)
            .ReturnsAsync(Guid.NewGuid());

        await _handler.Handle(cmd, CancellationToken.None);

        Assert.Equal(cmd.CustomerId, captured.CustomerId);
        Assert.Equal(cmd.FundId, captured.FundId);
        Assert.Equal(cmd.Amount, captured.Amount);
        Assert.Equal(cmd.Type, captured.Type);
        Assert.True((DateTime.UtcNow - captured.TransactionDate).TotalSeconds < 5);
    }

    [Fact]
    public async Task Handle_CallsRepositoryOnce()
    {
        var cmd = new CreateTransactionCommand
        {
            CustomerId = Guid.NewGuid(),
            FundId = 3,
            Amount = 200,
            Type = TransactionType.Opening
        };

        _repoMock.Setup(r => r.CreateTransactionAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Guid.NewGuid());

        await _handler.Handle(cmd, CancellationToken.None);

        _repoMock.Verify(r => r.CreateTransactionAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ReturnsGuidNotEmpty()
    {
        var cmd = new CreateTransactionCommand
        {
            CustomerId = Guid.NewGuid(),
            FundId = 4,
            Amount = 300,
            Type = TransactionType.Opening
        };

        var expectedId = Guid.NewGuid();
        _repoMock.Setup(r => r.CreateTransactionAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedId);

        var result = await _handler.Handle(cmd, CancellationToken.None);

        Assert.NotEqual(Guid.Empty, result);
    }

    [Fact]
    public async Task Handle_ThrowsException_WhenRepositoryFails()
    {
        var cmd = new CreateTransactionCommand
        {
            CustomerId = Guid.NewGuid(),
            FundId = 5,
            Amount = 400,
            Type = TransactionType.Cancellation
        };

        _repoMock.Setup(r => r.CreateTransactionAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Repository error"));

        await Assert.ThrowsAsync<Exception>(() =>
            _handler.Handle(cmd, CancellationToken.None));
      }
}      
