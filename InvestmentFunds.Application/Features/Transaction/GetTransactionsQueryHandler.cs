using InvestmentFunds.Application.Dtos;
using InvestmentFunds.Application.Interfaces;
using MediatR;

namespace InvestmentFunds.Application.Features.Transaction;

public class GetTransactionsQueryHandler : IRequestHandler<GetTransactionsQuery, List<TransactionDto>>
{
    private readonly ITransactionRepository _transactionRepository;

    public GetTransactionsQueryHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<List<TransactionDto>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
    {
        var transactions = await _transactionRepository.ListTransactionsByClientAsync(request.CustomerId, request.Type, cancellationToken);
        return transactions.Select(transaction => new TransactionDto
        {
            TransactionId = transaction.TransactionId,
            CustomerId = transaction.CustomerId,
            FundId = transaction.FundId,
            Amount = transaction.Amount,
            TransactionDate = transaction.TransactionDate,
            Type = transaction.Type
        }).ToList();
    }
}
