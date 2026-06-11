using MarketerSystem.Data.Context;
using MarketerSystem.Domain.Model;
using MarketerSystem.Domain.ResourceParameters;
using MarketerSystem.Repository.Repository;
using MarketerSystem.Service.Service;
using MarketerSystem.Tests.Infrastructure;
using Shouldly;

namespace MarketerSystem.Tests.Services;

/// <summary>
/// Runs against EF InMemory with real repositories so the queries
/// (Where/Include composition, change tracking, the single commit)
/// actually execute instead of being masked by mocks.
/// </summary>
public sealed class BonusPaymentServiceTests : IDisposable
{
    private readonly MarketerDBContext _db;
    private readonly BonusPaymentService _sut;

    public BonusPaymentServiceTests()
    {
        _db = InMemoryContextFactory.Create();
        _sut = new BonusPaymentService(
            _db,
            new BonusPaymentRepository(_db),
            new SellRepository(_db),
            new DistributorRepository(_db));
    }

    public void Dispose() => _db.Dispose();

    private static PaymentParameters JanuaryWindow() => new()
    {
        FromDate = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero),
        Todate = new DateTimeOffset(2026, 1, 31, 0, 0, 0, TimeSpan.Zero)
    };

    private async Task SeedDistributorAsync(int id, string? generationLinker, string firstName = "Test", string lastName = "Distributor")
    {
        _db.Distributors.Add(new Distributor
        {
            DistributorID = id,
            FirstName = firstName,
            LastName = lastName,
            BirthDate = new DateTime(1990, 1, 1),
            GenerationLinker = generationLinker
        });
        await _db.SaveChangesAsync();
    }

    private async Task SeedSellAsync(int id, int distributorId, decimal totalPrice, DateTimeOffset soldDate, bool usedForPayment = false)
    {
        _db.Sells.Add(new Sell
        {
            ID = id,
            DistributorID = distributorId,
            ProductID = 1,
            ProductTotalPrice = totalPrice,
            SoldDate = soldDate,
            UsedForPayment = usedForPayment
        });
        await _db.SaveChangesAsync();
    }

    [Fact]
    public async Task FilterPaymentsProducts_NullParameters_ThrowsArgumentNullException()
    {
        await Should.ThrowAsync<ArgumentNullException>(() => _sut.FilterPaymentsProducts(null!));
    }

    [Fact]
    public async Task GenerateBonusPaymentsForPeriodAsync_NullParameters_ThrowsArgumentNullException()
    {
        await Should.ThrowAsync<ArgumentNullException>(() => _sut.GenerateBonusPaymentsForPeriodAsync(null!));
    }

    [Fact]
    public async Task GenerateBonusPaymentsForPeriodAsync_PaysDirectSellerTenPercent_AndMarksSale()
    {
        await SeedDistributorAsync(7, generationLinker: null);
        await SeedSellAsync(1, distributorId: 7, totalPrice: 100m, soldDate: new DateTimeOffset(2026, 1, 5, 0, 0, 0, TimeSpan.Zero));

        var created = await _sut.GenerateBonusPaymentsForPeriodAsync(JanuaryWindow());

        var payment = created.ShouldHaveSingleItem();
        payment.DistributorID.ShouldBe(7);
        payment.BonusPay.ShouldBe(10m);

        _db.BonusPayments.Count().ShouldBe(1);
        _db.Sells.Single(s => s.ID == 1).UsedForPayment.ShouldBeTrue();
    }

    [Fact]
    public async Task GenerateBonusPaymentsForPeriodAsync_PaysUplineByLevel()
    {
        await SeedDistributorAsync(9, generationLinker: "1,2,3");
        await SeedSellAsync(1, distributorId: 9, totalPrice: 100m, soldDate: new DateTimeOffset(2026, 1, 5, 0, 0, 0, TimeSpan.Zero));

        var created = await _sut.GenerateBonusPaymentsForPeriodAsync(JanuaryWindow());

        created.Count.ShouldBe(3);
        created.Single(p => p.DistributorID == 9).BonusPay.ShouldBe(10m);  // direct seller
        created.Single(p => p.DistributorID == 3).BonusPay.ShouldBe(5m);   // immediate recommender
        created.Single(p => p.DistributorID == 2).BonusPay.ShouldBe(1m);   // grand recommender
        created.ShouldNotContain(p => p.DistributorID == 1);               // beyond paid levels
    }

    [Fact]
    public async Task GenerateBonusPaymentsForPeriodAsync_SkipsAlreadyPaidSales()
    {
        await SeedDistributorAsync(7, generationLinker: null);
        await SeedSellAsync(1, distributorId: 7, totalPrice: 100m,
            soldDate: new DateTimeOffset(2026, 1, 5, 0, 0, 0, TimeSpan.Zero), usedForPayment: true);

        var created = await _sut.GenerateBonusPaymentsForPeriodAsync(JanuaryWindow());

        created.ShouldBeEmpty();
        _db.BonusPayments.Count().ShouldBe(0);
    }

    [Fact]
    public async Task GenerateBonusPaymentsForPeriodAsync_SkipsSalesOutsideTheWindow()
    {
        await SeedDistributorAsync(7, generationLinker: null);
        await SeedSellAsync(1, distributorId: 7, totalPrice: 100m, soldDate: new DateTimeOffset(2026, 2, 5, 0, 0, 0, TimeSpan.Zero));

        var created = await _sut.GenerateBonusPaymentsForPeriodAsync(JanuaryWindow());

        created.ShouldBeEmpty();
        _db.Sells.Single(s => s.ID == 1).UsedForPayment.ShouldBeFalse();
    }

    [Fact]
    public async Task GenerateBonusPaymentsForPeriodAsync_MissingSellerRecord_StillPaysDirectSellerWithoutUpline()
    {
        // Sale references a distributor row that no longer exists — the direct
        // payment is still produced, only the upline walk is skipped.
        await SeedSellAsync(1, distributorId: 42, totalPrice: 100m, soldDate: new DateTimeOffset(2026, 1, 5, 0, 0, 0, TimeSpan.Zero));

        var created = await _sut.GenerateBonusPaymentsForPeriodAsync(JanuaryWindow());

        var payment = created.ShouldHaveSingleItem();
        payment.DistributorID.ShouldBe(42);
        payment.BonusPay.ShouldBe(10m);
    }

    private async Task SeedSamplePaymentsAsync()
    {
        await SeedDistributorAsync(1, null, "Zura", "Samkharadze");
        await SeedDistributorAsync(2, null, "Maiko", "Samkharadze");
        await SeedDistributorAsync(3, null, "Nika", "Beridze");

        _db.BonusPayments.AddRange(
            new BonusPayment { ID = 1, BonusPay = 25, DistributorID = 1 },
            new BonusPayment { ID = 2, BonusPay = 50, DistributorID = 2 },
            new BonusPayment { ID = 3, BonusPay = 100, DistributorID = 3 });
        await _db.SaveChangesAsync();
    }

    [Fact]
    public async Task FilterPaymentsProducts_NoFilters_ReturnsAllPayments()
    {
        await SeedSamplePaymentsAsync();

        var result = await _sut.FilterPaymentsProducts(new PaymentFilterParameters());

        result.Count.ShouldBe(3);
    }

    [Fact]
    public async Task FilterPaymentsProducts_MinPriceFilter_ExcludesLowerBonuses()
    {
        await SeedSamplePaymentsAsync();

        var result = await _sut.FilterPaymentsProducts(new PaymentFilterParameters { MinPrice = 50 });

        result.Count.ShouldBe(2);
        result.ShouldAllBe(p => p.BonusPay >= 50);
    }

    [Fact]
    public async Task FilterPaymentsProducts_MaxPriceFilter_ExcludesHigherBonuses()
    {
        await SeedSamplePaymentsAsync();

        var result = await _sut.FilterPaymentsProducts(new PaymentFilterParameters { MaxPrice = 50 });

        result.Count.ShouldBe(2);
        result.ShouldAllBe(p => p.BonusPay <= 50);
    }

    [Fact]
    public async Task FilterPaymentsProducts_NameFilter_MatchesDistributorFirstName()
    {
        await SeedSamplePaymentsAsync();

        var result = await _sut.FilterPaymentsProducts(new PaymentFilterParameters { Name = "Zura" });

        var only = result.ShouldHaveSingleItem();
        only.Distributor.ShouldNotBeNull();
        only.Distributor.FirstName.ShouldBe("Zura");
    }

    [Fact]
    public async Task FilterPaymentsProducts_LastNameFilter_MatchesDistributorLastName()
    {
        await SeedSamplePaymentsAsync();

        var result = await _sut.FilterPaymentsProducts(new PaymentFilterParameters { LastName = "Beridze" });

        var only = result.ShouldHaveSingleItem();
        only.Distributor.ShouldNotBeNull();
        only.Distributor.LastName.ShouldBe("Beridze");
    }
}
