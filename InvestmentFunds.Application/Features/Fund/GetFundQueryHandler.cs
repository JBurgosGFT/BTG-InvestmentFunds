using InvestmentFunds.Application.Dtos;
using InvestmentFunds.Application.Interfaces;
using MediatR;

namespace InvestmentFunds.Application.Features.Fund;

public class GetFundQueryHandler : IRequestHandler<GetFundQuery, FundDto>
{
    private readonly IFundRepository _fundRepository;

    public GetFundQueryHandler(IFundRepository fundRepository)
    {
        _fundRepository = fundRepository;
    }

    public async Task<FundDto> Handle(GetFundQuery request, CancellationToken cancellationToken)
    {
        var fund = await _fundRepository.GetFundAsync(request.FundId, cancellationToken);
        if (fund == null)
            throw new Exception($"Fondo no encontrado.");

        var fundDto = new FundDto
        {
            FundId = fund.FundId,
            Name = fund.Name,
            Category = fund.Category,
            MinAmount = fund.MinAmount
        };

        return fundDto;
    }
}
