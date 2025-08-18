namespace InvestmentFunds.Domain.Entities;

public class Subscription
{
    public Guid SubscriptionId { get; set; } = Guid.NewGuid();
    public int FundId { get; set; }
    public DateTime SubscriptionDate { get; set; } = DateTime.UtcNow;
    public int Amount { get; set; }
    public bool IsActive { get; set; } = true;

    public void Cancel()
    {
        if (!IsActive) throw new InvalidOperationException("ALREADY_CANCELLED");
        IsActive = false;
    }
}
