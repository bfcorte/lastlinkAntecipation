using LastLink.Anticipation.Application.DTOs;
using LastLink.Anticipation.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace LastLink.Anticipation.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AnticipationsController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<AnticipationResponseDto>> Create([FromServices] CreateAnticipationHandler handler, [FromBody] CreateAnticipationDto body, CancellationToken ct)
    {
        try { var result = await handler.HandleAsync(body, ct); return CreatedAtAction(nameof(GetByCreator), new { creator_id = result.CreatorId }, result); }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
        catch (ArgumentException ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AnticipationResponseDto>>> GetByCreator([FromServices] ListByCreatorHandler handler, [FromQuery(Name = "creator_id")] Guid creatorId, CancellationToken ct)
        => Ok(await handler.HandleAsync(creatorId, ct));

    [HttpPost("{id:guid}/approve")]
    public async Task<ActionResult<AnticipationResponseDto>> Approve([FromServices] ApproveRejectHandler handler, Guid id, CancellationToken ct)
    {
        try { return Ok(await handler.ApproveAsync(id, ct)); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpPost("{id:guid}/reject")]
    public async Task<ActionResult<AnticipationResponseDto>> Reject([FromServices] ApproveRejectHandler handler, Guid id, CancellationToken ct)
    {
        try { return Ok(await handler.RejectAsync(id, ct)); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
    }
}
