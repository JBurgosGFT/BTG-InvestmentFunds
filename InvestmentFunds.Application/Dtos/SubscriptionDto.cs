namespace InvestmentFunds.Application.Dtos;

public class SubscriptionDto
{
    public Guid SubscriptionId { get; set; } = Guid.NewGuid();
    public int FundId { get; set; }
    public DateTime SubscriptionDate { get; set; } = DateTime.UtcNow;
    public int Amount { get; set; }
    public bool IsActive { get; set; } = true;
    public Guid CustomerId { get; set; }
}
