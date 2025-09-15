using LastLink.Anticipation.Application.DTOs;
using LastLink.Anticipation.Application.UseCases;
using LastLink.Anticipation.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LastLink.Anticipation.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AnticipationsController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<AnticipationResponseDto>> Create(
        [FromServices] CreateAnticipationHandler handler,
        [FromBody] CreateAnticipationDto body,
        CancellationToken ct)
    {
        var result = await handler.HandleAsync(body, ct);
        return CreatedAtAction(nameof(GetByCreator), new { creator_id = result.CreatorId }, result);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AnticipationResponseDto>>> GetByCreator(
        [FromServices] ListByCreatorHandler handler,
        [FromQuery(Name = "creator_id")] Guid creatorId,
        CancellationToken ct)
    {
        var list = await handler.HandleAsync(creatorId, ct);
        return Ok(list);
    }

    [HttpPost("{id:guid}/approve")]
    public async Task<ActionResult<AnticipationResponseDto>> Approve(
        [FromServices] ApproveRejectHandler handler,
        Guid id,
        CancellationToken ct)
    {
        var result = await handler.ApproveAsync(id, ct);
        return Ok(result);
    }

    [HttpPost("{id:guid}/reject")]
    public async Task<ActionResult<AnticipationResponseDto>> Reject(
        [FromServices] ApproveRejectHandler handler,
        Guid id,
        CancellationToken ct)
    {
        var result = await handler.RejectAsync(id, ct);
        return Ok(result);
    }

    [HttpGet("simulate")]
    public ActionResult<object> Simulate([FromQuery(Name = "valor_solicitado")] decimal valorSolicitado)
    {
        if (valorSolicitado <= 100m) return BadRequest(new { message = "Min R$100,00" });
        var net = AnticipationRequest.CalculateNet(valorSolicitado);
        return Ok(new { valor_bruto = valorSolicitado, taxa = AnticipationRequest.FeeRate, valor_liquido = net, status = "simulado" });
    }
}
