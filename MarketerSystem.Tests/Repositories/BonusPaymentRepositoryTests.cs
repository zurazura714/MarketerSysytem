using FluentAssertions;
using MarketerSystem.Domain.Model;
using MarketerSystem.Repository.Repository;
using MarketerSystem.Tests.Infrastructure;

namespace MarketerSystem.Tests.Repositories;

public class BonusPaymentRepositoryTests
{
    [Fact]
    public async Task SaveAsync_AddsEntityAndCommitMakesItRetrievable()
    {
        using var db = InMemoryContextFactory.Create();
        var repo = new BonusPaymentRepository(db);

        var payment = new BonusPayment
        {
            ID = 1,
            BonusPay = 42,
            DistributorID = 1,
            FromDate = DateTimeOffset.UtcNow.AddDays(-1),
            ToDate = DateTimeOffset.UtcNow
        };

        await repo.SaveAsync(payment);
        await db.CommitAsync();

        var stored = await repo.FetchAsync(1);
        stored.Should().NotBeNull();
        stored!.BonusPay.Should().Be(42);
    }

    [Fact]
    public async Task FetchAsync_UnknownId_ReturnsNull()
    {
        using var db = InMemoryContextFactory.Create();
        var repo = new BonusPaymentRepository(db);

        var result = await repo.FetchAsync(404);

        result.Should().BeNull();
    }

    [Fact]
    public async Task SetAsync_ReturnsAllPersistedPayments()
    {
        using var db = InMemoryContextFactory.Create();
        var repo = new BonusPaymentRepository(db);

        await repo.SaveAsync(new BonusPayment { ID = 1, BonusPay = 10, DistributorID = 1 });
        await repo.SaveAsync(new BonusPayment { ID = 2, BonusPay = 20, DistributorID = 2 });
        await db.CommitAsync();

        var all = await repo.SetAsync();

        all.Should().HaveCount(2);
    }

    [Fact]
    public async Task DeleteAsync_RemovesEntityFromContext()
    {
        using var db = InMemoryContextFactory.Create();
        var repo = new BonusPaymentRepository(db);

        var payment = new BonusPayment { ID = 1, BonusPay = 10, DistributorID = 1 };
        await repo.SaveAsync(payment);
        await db.CommitAsync();

        await repo.DeleteAsync(payment);
        await db.CommitAsync();

        var result = await repo.FetchAsync(1);
        result.Should().BeNull();
    }
}
