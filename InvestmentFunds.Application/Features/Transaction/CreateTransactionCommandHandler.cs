
using InvestmentFunds.Application.Interfaces;
using MediatR;

namespace InvestmentFunds.Application.Features.Transaction;

public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, Guid>
{
    private readonly ITransactionRepository _transactionRepository;

    public CreateTransactionCommandHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<Guid> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = new Domain.Entities.Transaction
        {
            CustomerId = request.CustomerId,
            FundId = request.FundId,
            Amount = request.Amount,
            TransactionDate = DateTime.UtcNow,
            Type = request.Type
        };

        Guid transactionId = await _transactionRepository.CreateTransactionAsync(transaction, cancellationToken);

        return transactionId;
    }
}
