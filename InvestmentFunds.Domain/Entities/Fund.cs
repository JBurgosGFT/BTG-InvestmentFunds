namespace InvestmentFunds.Domain.Entities;

public class Fund
{
    public int FundId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int MinAmount { get; set; }
}
