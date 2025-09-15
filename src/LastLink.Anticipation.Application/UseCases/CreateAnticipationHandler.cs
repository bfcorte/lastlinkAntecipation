using LastLink.Anticipation.Application.DTOs;
using LastLink.Anticipation.Domain.Entities;
using LastLink.Anticipation.Domain.Repositories;

namespace LastLink.Anticipation.Application.UseCases;

public class CreateAnticipationHandler
{
    private readonly IAnticipationRepository _repo;
    public CreateAnticipationHandler(IAnticipationRepository repo) => _repo = repo;

    public async Task<AnticipationResponseDto> HandleAsync(CreateAnticipationDto input, CancellationToken ct = default)
    {
        if (await _repo.HasPendingForCreatorAsync(input.CreatorId, ct))
            throw new InvalidOperationException("Já existe pendente.");

        var entity = new AnticipationRequest(input.CreatorId, input.ValorSolicitado, input.DataSolicitacao);
        await _repo.AddAsync(entity, ct);
        await _repo.SaveChangesAsync(ct);

        return new AnticipationResponseDto(entity.Id, entity.CreatorId, entity.RequestedAmount, entity.NetAmount, entity.RequestedAt, entity.Status);
    }
}
