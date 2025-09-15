using LastLink.Anticipation.Domain.Entities;
using LastLink.Anticipation.Domain.Enums;
using LastLink.Anticipation.Domain.Repositories;
using LastLink.Anticipation.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LastLink.Anticipation.Infrastructure.Repositories;

public class AnticipationRepository : IAnticipationRepository
{
    private readonly AnticipationDbContext _db;
    public AnticipationRepository(AnticipationDbContext db) => _db = db;

    public async Task AddAsync(AnticipationRequest request, CancellationToken ct = default)
        => await _db.Anticipations.AddAsync(request, ct);

    public async Task<IReadOnlyList<AnticipationRequest>> GetByCreatorAsync(Guid creatorId, CancellationToken ct = default)
        => await _db.Anticipations.AsNoTracking().Where(x => x.CreatorId == creatorId).OrderByDescending(x => x.RequestedAt).ToListAsync(ct);

    public async Task<bool> HasPendingForCreatorAsync(Guid creatorId, CancellationToken ct = default)
        => await _db.Anticipations.AnyAsync(x => x.CreatorId == creatorId && x.Status == AnticipationStatus.Pending, ct);

    public async Task<AnticipationRequest?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _db.Anticipations.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task<AnticipationRequest?> GetByIdForUpdateAsync(Guid id, CancellationToken ct = default)
        => await _db.Anticipations.FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task SaveChangesAsync(CancellationToken ct = default)
        => await _db.SaveChangesAsync(ct);
}
