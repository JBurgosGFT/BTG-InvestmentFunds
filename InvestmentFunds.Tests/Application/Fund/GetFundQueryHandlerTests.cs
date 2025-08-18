using Moq;
using InvestmentFunds.Application.Features.Fund;
using InvestmentFunds.Application.Interfaces;
using InvestmentFunds.Domain.Entities;

public class GetFundQueryHandlerTests
{
    private readonly Mock<IFundRepository> _repoMock;
    private readonly GetFundQueryHandler _handler;

    public GetFundQueryHandlerTests()
    {
        _repoMock = new Mock<IFundRepository>();
        _handler = new GetFundQueryHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsFundDto_WhenFundExists()
    {
        var fundId = 1;
        _repoMock.Setup(r => r.GetFundAsync(fundId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Fund { FundId = fundId, Name = "Fondo A", Category = "Renta Fija", MinAmount = 1000 });

        var result = await _handler.Handle(new GetFundQuery { FundId = fundId }, CancellationToken.None);

        Assert.Equal(fundId, result.FundId);
        Assert.Equal("Fondo A", result.Name);
        Assert.Equal("Renta Fija", result.Category);
        Assert.Equal(1000, result.MinAmount);
    }

    [Fact]
    public async Task Handle_ThrowsException_WhenFundDoesNotExist()
    {
        var fundId = 2;
        _repoMock.Setup(r => r.GetFundAsync(fundId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Fund)null);

        await Assert.ThrowsAsync<Exception>(() =>
            _handler.Handle(new GetFundQuery { FundId = fundId }, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ReturnsCorrectCategory()
    {
        var fundId = 3;
        _repoMock.Setup(r => r.GetFundAsync(fundId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Fund { FundId = fundId, Name = "Fondo B", Category = "Acciones", MinAmount = 500 });

        var result = await _handler.Handle(new GetFundQuery { FundId = fundId }, CancellationToken.None);

        Assert.Equal("Acciones", result.Category);
    }

    [Fact]
    public async Task Handle_ReturnsCorrectMinAmount()
    {
        var fundId = 4;
        _repoMock.Setup(r => r.GetFundAsync(fundId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Fund { FundId = fundId, Name = "Fondo C", Category = "Mixto", MinAmount = 2000 });

        var result = await _handler.Handle(new GetFundQuery { FundId = fundId }, CancellationToken.None);

        Assert.Equal(2000, result.MinAmount);
    }

    [Fact]
    public async Task Handle_CallsRepositoryOnce()
    {
        var fundId = 5;
        _repoMock.Setup(r => r.GetFundAsync(fundId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Fund { FundId = fundId, Name = "Fondo D", Category = "Renta Variable", MinAmount = 3000 });

        await _handler.Handle(new GetFundQuery { FundId = fundId }, CancellationToken.None);

        _repoMock.Verify(r => r.GetFundAsync(fundId, It.IsAny<CancellationToken>()), Times.Once);
    }
}