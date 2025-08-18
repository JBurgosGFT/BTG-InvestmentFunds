using Moq;
using InvestmentFunds.Application.Features.Transaction;
using InvestmentFunds.Application.Interfaces;
using InvestmentFunds.Domain.Entities;
using InvestmentFunds.Domain.Enums;

public class GetTransactionsQueryHandlerTests
{
    private readonly Mock<ITransactionRepository> _repoMock;
    private readonly GetTransactionsQueryHandler _handler;

    public GetTransactionsQueryHandlerTests()
    {
        _repoMock = new Mock<ITransactionRepository>();
        _handler = new GetTransactionsQueryHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsEmptyList_WhenNoTransactions()
    {
        var query = new GetTransactionsQuery
        {
            CustomerId = Guid.NewGuid(),
            Type = TransactionType.Opening
        };

        _repoMock.Setup(r => r.ListTransactionsByClientAsync(query.CustomerId, query.Type, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Transaction>());

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.Empty(result);
    }

    [Fact]
    public async Task Handle_ReturnsCorrectTransactionDtos()
    {
        var query = new GetTransactionsQuery
        {
            CustomerId = Guid.NewGuid(),
            Type = TransactionType.Opening
        };

        var transactions = new List<Transaction>
        {
            new Transaction { TransactionId = Guid.NewGuid(), CustomerId = query.CustomerId, FundId = 1, Amount = 100, TransactionDate = DateTime.UtcNow.AddDays(-5), Type = TransactionType.Opening },
            new Transaction { TransactionId = Guid.NewGuid(), CustomerId = query.CustomerId, FundId = 2, Amount = 200, TransactionDate = DateTime.UtcNow.AddDays(-3), Type = TransactionType.Opening }
        };

        _repoMock.Setup(r => r.ListTransactionsByClientAsync(query.CustomerId, query.Type, It.IsAny<CancellationToken>()))
            .ReturnsAsync(transactions);

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.Equal(transactions[0].TransactionId, result[0].TransactionId);
        Assert.Equal(transactions[1].TransactionId, result[1].TransactionId);
    }

    [Fact]
    public async Task Handle_FiltersByType()
    {
        var query = new GetTransactionsQuery
        {
            CustomerId = Guid.NewGuid(),
            Type = TransactionType.Cancellation
        };

        var transactions = new List<Transaction>
        {
            new Transaction { TransactionId = Guid.NewGuid(), CustomerId = query.CustomerId, FundId = 1, Amount = 100, TransactionDate = DateTime.UtcNow.AddDays(-5), Type = TransactionType.Cancellation }
        };

        _repoMock.Setup(r => r.ListTransactionsByClientAsync(query.CustomerId, query.Type, It.IsAny<CancellationToken>()))
            .ReturnsAsync(transactions);

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.Single(result);
        Assert.Equal(TransactionType.Cancellation, result[0].Type);
    }

    [Fact]
    public async Task Handle_CallsRepositoryOnce()
    {
        var query = new GetTransactionsQuery
        {
            CustomerId = Guid.NewGuid(),
            Type = TransactionType.Opening
        };

        _repoMock.Setup(r => r.ListTransactionsByClientAsync(query.CustomerId, query.Type, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Transaction>());

        await _handler.Handle(query, CancellationToken.None);

        _repoMock.Verify(r => r.ListTransactionsByClientAsync(query.CustomerId, query.Type, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ReturnsCorrectAmounts()
    {
        var query = new GetTransactionsQuery
        {
            CustomerId = Guid.NewGuid(),
            Type = TransactionType.Opening
        };

        var transactions = new List<Transaction>
        {
            new Transaction { TransactionId = Guid.NewGuid(), CustomerId = query.CustomerId, FundId = 1, Amount = 150, TransactionDate = DateTime.UtcNow.AddDays(-2), Type = TransactionType.Opening }
        };

        _repoMock.Setup(r => r.ListTransactionsByClientAsync(query.CustomerId, query.Type, It.IsAny<CancellationToken>()))
            .ReturnsAsync(transactions);

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.Equal(150,result[0].Amount);
    }
}