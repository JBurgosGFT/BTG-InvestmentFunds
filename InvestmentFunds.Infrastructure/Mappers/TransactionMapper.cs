using InvestmentFunds.Domain.Entities;

public static class TransactionMapper
{
    public static TransactionDynamoModel ToDynamoModel(Transaction transaction)
    {
        return new TransactionDynamoModel
        {
            TransactionId = transaction.TransactionId,
            CustomerId = transaction.CustomerId,
            FundId = transaction.FundId,
            TransactionDate = transaction.TransactionDate,
            Amount = transaction.Amount,
            Type = transaction.Type
        };
    }

    public static Transaction ToDomainEntity(TransactionDynamoModel model)
    {
        return new Transaction
        {
            TransactionId = model.TransactionId,
            CustomerId = model.CustomerId,
            FundId = model.FundId,
            TransactionDate = model.TransactionDate,
            Amount = model.Amount,
            Type = model.Type
        };
    }
}