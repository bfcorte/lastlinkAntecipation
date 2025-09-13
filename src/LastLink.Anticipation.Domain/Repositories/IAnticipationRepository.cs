using LastLink.Anticipation.Domain.Entities;
namespace LastLink.Anticipation.Domain.Repositories;
public interface IAnticipationRepository
{
    Task AddAsync(AnticipationRequest request, CancellationToken ct = default);
    Task<AnticipationRequest?> GetByIdAsync(Guid id, CancellationToken ct = default); // read, no-tracking
    Task<AnticipationRequest?> GetByIdForUpdateAsync(Guid id, CancellationToken ct = default); // tracked for updates
    Task<IReadOnlyList<AnticipationRequest>> GetByCreatorAsync(Guid creatorId, CancellationToken ct = default);
    Task<bool> HasPendingForCreatorAsync(Guid creatorId, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
