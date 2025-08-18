using InvestmentFunds.Domain.Enums;

namespace InvestmentFunds.Application.Dtos;

public class CustomerDto
{
    public Guid CustomerId { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public int Balance { get; set; } = 500000;
    public NotificationPreference NotificationPreference { get; set; } = NotificationPreference.Email;
}
