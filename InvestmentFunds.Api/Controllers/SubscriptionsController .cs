using InvestmentFunds.Application.Features.Subscription;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvestmentFunds.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SubscriptionsController : ControllerBase
{
    private readonly IMediator _mediator;
    public SubscriptionsController(IMediator mediator) => _mediator = mediator;

    [HttpPost("subscribe")]
    public async Task<IActionResult> Subscribe([FromBody] SubscribeCommand cmd, CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(cmd, cancellationToken);
        if (id == Guid.Empty)
            return BadRequest(new { message = "No se pudo crear la suscripción." });

        return Ok(new { subscriptionId = id });
    }

    [HttpGet("unsubscribe/{subscriptionId}")]
    public async Task<IActionResult> CancelSubscription([FromRoute] Guid subscriptionId, CancellationToken cancellationToken)
    {
        var cmd = new UnsubscribeCommand { SubscriptionId = subscriptionId };
        var id = await _mediator.Send(cmd, cancellationToken);
        if (id == Guid.Empty)
            return NoContent();

        return Ok(new { subscriptionId = id });
    }
}
