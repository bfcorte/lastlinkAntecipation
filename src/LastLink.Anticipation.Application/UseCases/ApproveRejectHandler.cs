using LastLink.Anticipation.Application.DTOs;
using LastLink.Anticipation.Domain.Repositories;

namespace LastLink.Anticipation.Application.UseCases;

public class ApproveRejectHandler
{
    private readonly IAnticipationRepository _repo;
    public ApproveRejectHandler(IAnticipationRepository repo) => _repo = repo;

    public async Task<AnticipationResponseDto> ApproveAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _repo.GetByIdForUpdateAsync(id, ct) ?? throw new KeyNotFoundException("Solicitação não encontrada.");
        entity.Approve();
        await _repo.SaveChangesAsync(ct);
        return new AnticipationResponseDto(entity.Id, entity.CreatorId, entity.RequestedAmount, entity.NetAmount, entity.RequestedAt, entity.Status);
    }

    public async Task<AnticipationResponseDto> RejectAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _repo.GetByIdForUpdateAsync(id, ct) ?? throw new KeyNotFoundException("Solicitação não encontrada.");
        entity.Reject();
        await _repo.SaveChangesAsync(ct);
        return new AnticipationResponseDto(entity.Id, entity.CreatorId, entity.RequestedAmount, entity.NetAmount, entity.RequestedAt, entity.Status);
    }
}
