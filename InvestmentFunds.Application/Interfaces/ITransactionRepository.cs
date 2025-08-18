using InvestmentFunds.Domain.Entities;
using InvestmentFunds.Domain.Enums;

namespace InvestmentFunds.Application.Interfaces;

public interface ITransactionRepository
{
    Task<Guid> CreateTransactionAsync(Transaction transaction, CancellationToken cancellationToken = default);
    Task<IEnumerable<Transaction>> ListTransactionsByClientAsync(Guid clientId, TransactionType type, CancellationToken cancellationToken = default);
    Task<IEnumerable<Transaction>> ListAllTransactionsAsync(DateTime startDate, DateTime endDate, TransactionType type, CancellationToken cancellationToken = default);
}
