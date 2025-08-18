using InvestmentFunds.Domain.Entities;

namespace InvestmentFunds.Application.Interfaces;

public interface IFundRepository
{
    Task<Fund?> GetFundAsync(int fundId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Fund>> ListFundsAsync(CancellationToken cancellationToken = default);
}
