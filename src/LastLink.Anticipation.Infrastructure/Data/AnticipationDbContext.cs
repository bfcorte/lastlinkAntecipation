using LastLink.Anticipation.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LastLink.Anticipation.Infrastructure.Data;

public class AnticipationDbContext : DbContext
{
    public AnticipationDbContext(DbContextOptions<AnticipationDbContext> options) : base(options) { }

    public DbSet<AnticipationRequest> Anticipations => Set<AnticipationRequest>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<AnticipationRequest>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.RequestedAmount).HasPrecision(18,2);
            e.Property(x => x.NetAmount).HasPrecision(18,2);
            e.HasIndex(x => new { x.CreatorId, x.Status });
        });
    }
}
