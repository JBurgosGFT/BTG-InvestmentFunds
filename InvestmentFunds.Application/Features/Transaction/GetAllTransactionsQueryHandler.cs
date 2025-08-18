using InvestmentFunds.Application.Dtos;
using InvestmentFunds.Application.Interfaces;
using MediatR;

namespace InvestmentFunds.Application.Features.Transaction;

public class GetAllTransactionsQueryHandler : IRequestHandler<GetAllTransactionsQuery, List<TransactionDto>>
{
    private readonly ITransactionRepository _transactionRepository;

    public GetAllTransactionsQueryHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<List<TransactionDto>> Handle(GetAllTransactionsQuery request, CancellationToken cancellationToken)
    {
        var transactions = await _transactionRepository.ListAllTransactionsAsync(request.StartDate, request.EndDate, request.Type, cancellationToken);
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
