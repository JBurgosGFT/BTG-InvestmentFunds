using InvestmentFunds.Application.Features.Transaction;
using InvestmentFunds.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvestmentFunds.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TransactionsController : ControllerBase
{
    private readonly IMediator _mediator;
    public TransactionsController(IMediator mediator) => _mediator = mediator;

    [HttpGet("by-customer")]
    public async Task<IActionResult> GetTransactions([FromQuery] Guid customerId, [FromQuery] TransactionType type)
    {
        var result = await _mediator.Send(new GetTransactionsQuery { CustomerId = customerId, Type = type });
        return Ok(result);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllTransactions([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] TransactionType type)
    {
        var result = await _mediator.Send(new GetAllTransactionsQuery { StartDate = startDate, EndDate = endDate, Type = type });
        return Ok(result);
    }
}
