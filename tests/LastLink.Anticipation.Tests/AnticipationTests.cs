using System;
using System.Threading.Tasks;
using FluentAssertions;
using LastLink.Anticipation.Application.DTOs;
using LastLink.Anticipation.Application.UseCases;
using LastLink.Anticipation.Domain.Enums;
using LastLink.Anticipation.Domain.Repositories;
using LastLink.Anticipation.Infrastructure.Data;
using LastLink.Anticipation.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LastLink.Anticipation.Tests;

public class AnticipationTests
{
    private (CreateAnticipationHandler create, ListByCreatorHandler list, ApproveRejectHandler approve, IAnticipationRepository repo) BuildHandlers()
    {
        var options = new DbContextOptionsBuilder<AnticipationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var db = new AnticipationDbContext(options);
        var repo = new AnticipationRepository(db);
        return (new CreateAnticipationHandler(repo), new ListByCreatorHandler(repo), new ApproveRejectHandler(repo), repo);
    }

    [Fact] public async Task Should_Create_Pending_With_Fee()
    {
        var (create, _, _, _) = BuildHandlers();
        var creator = Guid.NewGuid();
        var dto = new CreateAnticipationDto(creator, 200m, DateTime.UtcNow);
        var res = await create.HandleAsync(dto);
        res.Status.Should().Be(AnticipationStatus.Pending);
        res.ValorLiquido.Should().Be(190m);
    }

    [Fact] public async Task Should_Not_Allow_Value_Less_Or_Equal_100()
    {
        var (create, _, _, _) = BuildHandlers();
        var creator = Guid.NewGuid();
        var dto = new CreateAnticipationDto(creator, 100m, DateTime.UtcNow);
        Func<Task> act = async () => await create.HandleAsync(dto);
        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact] public async Task Should_Block_If_Pending_Exists()
    {
        var (create, _, _, _) = BuildHandlers();
        var creator = Guid.NewGuid();
        await create.HandleAsync(new CreateAnticipationDto(creator, 200m, DateTime.UtcNow));
        Func<Task> act = async () => await create.HandleAsync(new CreateAnticipationDto(creator, 300m, DateTime.UtcNow));
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact] public async Task Should_Approve_And_Reject()
    {
        var (create, _, approve, _) = BuildHandlers();
        var creator = Guid.NewGuid();
        var created = await create.HandleAsync(new CreateAnticipationDto(creator, 300m, DateTime.UtcNow));
        var approved = await approve.ApproveAsync(created.Id);
        approved.Status.Should().Be(AnticipationStatus.Approved);
        Func<Task> act = async () => await approve.RejectAsync(created.Id);
        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}
