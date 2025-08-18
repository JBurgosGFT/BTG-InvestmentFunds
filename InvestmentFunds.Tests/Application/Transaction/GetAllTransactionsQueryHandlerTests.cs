using Moq;
using InvestmentFunds.Application.Features.Transaction;
using InvestmentFunds.Application.Interfaces;
using InvestmentFunds.Domain.Entities;
using InvestmentFunds.Domain.Enums;

public class GetAllTransactionsQueryHandlerTests
{
    private readonly Mock<ITransactionRepository> _repoMock;
    private readonly GetAllTransactionsQueryHandler _handler;

    public GetAllTransactionsQueryHandlerTests()
    {
        _repoMock = new Mock<ITransactionRepository>();
        _handler = new GetAllTransactionsQueryHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsEmptyList_WhenNoTransactions()
    {
        var query = new GetAllTransactionsQuery
        {
            StartDate = DateTime.UtcNow.AddDays(-10),
            EndDate = DateTime.UtcNow,
            Type = TransactionType.Opening
        };

        _repoMock.Setup(r => r.ListAllTransactionsAsync(query.StartDate, query.EndDate, query.Type, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Transaction>());

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.Empty(result);
    }

    [Fact]
    public async Task Handle_ReturnsCorrectTransactionDtos()
    {
        var query = new GetAllTransactionsQuery
        {
            StartDate = DateTime.UtcNow.AddDays(-10),
            EndDate = DateTime.UtcNow,
            Type = TransactionType.Opening
        };

        var transactions = new List<Transaction>
        {
            new Transaction { TransactionId = Guid.NewGuid(), CustomerId = Guid.NewGuid(), FundId = 1, Amount = 100, TransactionDate = DateTime.UtcNow.AddDays(-5), Type = TransactionType.Opening },
            new Transaction { TransactionId = Guid.NewGuid(), CustomerId = Guid.NewGuid(), FundId = 2, Amount = 200, TransactionDate = DateTime.UtcNow.AddDays(-3), Type = TransactionType.Opening }
        };

        _repoMock.Setup(r => r.ListAllTransactionsAsync(query.StartDate, query.EndDate, query.Type, It.IsAny<CancellationToken>()))
            .ReturnsAsync(transactions);

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.Equal(transactions[0].TransactionId, result[0].TransactionId);
        Assert.Equal(transactions[1].TransactionId, result[1].TransactionId);
    }

    [Fact]
    public async Task Handle_FiltersByType()
    {
        var query = new GetAllTransactionsQuery
        {
            StartDate = DateTime.UtcNow.AddDays(-10),
            EndDate = DateTime.UtcNow,
            Type = TransactionType.Cancellation
        };

        var transactions = new List<Transaction>
        {
            new Transaction { TransactionId = Guid.NewGuid(), CustomerId = Guid.NewGuid(), FundId = 1, Amount = 100, TransactionDate = DateTime.UtcNow.AddDays(-5), Type = TransactionType.Cancellation }
        };

        _repoMock.Setup(r => r.ListAllTransactionsAsync(query.StartDate, query.EndDate, query.Type, It.IsAny<CancellationToken>()))
            .ReturnsAsync(transactions);

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.Single(result);
        Assert.Equal(TransactionType.Cancellation, result[0].Type);
    }

    [Fact]
    public async Task Handle_CallsRepositoryOnce()
    {
        var query = new GetAllTransactionsQuery
        {
            StartDate = DateTime.UtcNow.AddDays(-10),
            EndDate = DateTime.UtcNow,
            Type = TransactionType.Opening
        };

        _repoMock.Setup(r => r.ListAllTransactionsAsync(query.StartDate, query.EndDate, query.Type, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Transaction>());

        await _handler.Handle(query, CancellationToken.None);

        _repoMock.Verify(r => r.ListAllTransactionsAsync(query.StartDate, query.EndDate, query.Type, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ReturnsCorrectAmounts()
    {
        var query = new GetAllTransactionsQuery
        {
            StartDate = DateTime.UtcNow.AddDays(-10),
            EndDate = DateTime.UtcNow,
            Type = TransactionType.Opening
        };

        var transactions = new List<Transaction>
        {
            new Transaction { TransactionId = Guid.NewGuid(), CustomerId = Guid.NewGuid(), FundId = 1, Amount = 150, TransactionDate = DateTime.UtcNow.AddDays(-2), Type = TransactionType.Opening }
        };

        _repoMock.Setup(r => r.ListAllTransactionsAsync(query.StartDate, query.EndDate, query.Type, It.IsAny<CancellationToken>()))
            .ReturnsAsync(transactions);

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.Equal(150, result[0].Amount);
    }
}