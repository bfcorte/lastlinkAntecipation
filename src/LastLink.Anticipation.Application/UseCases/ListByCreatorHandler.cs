using LastLink.Anticipation.Application.DTOs;
using LastLink.Anticipation.Domain.Repositories;
namespace LastLink.Anticipation.Application.UseCases;
public class ListByCreatorHandler
{
    private readonly IAnticipationRepository _repo;
    public ListByCreatorHandler(IAnticipationRepository repo) => _repo = repo;
    public async Task<IReadOnlyList<AnticipationResponseDto>> HandleAsync(Guid creatorId, CancellationToken ct = default)
    {
        var list = await _repo.GetByCreatorAsync(creatorId, ct);
        return list.Select(e => new AnticipationResponseDto(e.Id, e.CreatorId, e.RequestedAmount, e.NetAmount, e.RequestedAt, e.Status)).ToList();
    }
}
