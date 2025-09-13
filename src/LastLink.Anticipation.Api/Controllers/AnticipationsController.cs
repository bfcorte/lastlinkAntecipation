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
        try
        {
            var result = await handler.HandleAsync(body, ct);
            return CreatedAtAction(nameof(GetByCreator), new { creator_id = result.CreatorId }, result);
        }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
        catch (ArgumentException ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpGet]
    public ActionResult<string> GetByCreator() => Ok("Em breve: listagem");
}
