using InvestmentFunds.Domain.Enums;

namespace InvestmentFunds.Domain.Entities;

public class Transaction
{
    public Guid TransactionId { get; set; } = Guid.NewGuid();
    public int FundId { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
    public int Amount { get; set; }
    public TransactionType Type { get; set; } = TransactionType.Opening;
}
