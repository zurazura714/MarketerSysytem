using MarketerSystem.Data.Context;
using MarketerSystem.Domain.Model;
using MarketerSystem.Domain.ResourceParameters;
using MarketerSystem.Repository.Repository;
using MarketerSystem.Service.Service;
using MarketerSystem.Tests.Infrastructure;
using Shouldly;

namespace MarketerSystem.Tests.Services;

public sealed class SellServiceTests : IDisposable
{
    private readonly MarketerDBContext _db;
    private readonly SellService _sut;

    public SellServiceTests()
    {
        _db = InMemoryContextFactory.Create();
        _sut = new SellService(
            _db,
            new SellRepository(_db),
            new ProductRepository(_db),
            new DistributorRepository(_db));
    }

    public void Dispose() => _db.Dispose();

    private async Task SeedProductAndDistributorAsync(decimal price = 10m)
    {
        _db.Products.Add(new Product { ID = 1, Name = "Pen", Price = price });
        _db.Distributors.Add(new Distributor
        {
            DistributorID = 1,
            FirstName = "Zura",
            LastName = "Samkharadze",
            BirthDate = new DateTime(1990, 1, 1)
        });
        await _db.SaveChangesAsync();
    }

    [Fact]
    public async Task CreateSellAsync_Null_ThrowsArgumentNullException()
    {
        await Should.ThrowAsync<ArgumentNullException>(() => _sut.CreateSellAsync(null!));
    }

    [Fact]
    public async Task CreateSellAsync_SnapshotsPriceFromProductAndPersists()
    {
        await SeedProductAndDistributorAsync(price: 14m);

        var created = await _sut.CreateSellAsync(new Sell { ProductID = 1, DistributorID = 1 });

        created.ProductPrice.ShouldBe(14m);
        created.ProductUnitPrice.ShouldBe(14m);
        created.ProductTotalPrice.ShouldBe(14m);
        created.Product.ShouldNotBeNull();
        created.Distributor.ShouldNotBeNull();
        _db.Sells.Count().ShouldBe(1);
    }

    [Fact]
    public async Task CreateSellAsync_MissingProduct_ThrowsArgumentException()
    {
        var ex = await Should.ThrowAsync<ArgumentException>(
            () => _sut.CreateSellAsync(new Sell { ProductID = 999, DistributorID = 1 }));
        ex.Message.ShouldContain("Product 999");
    }

    [Fact]
    public async Task CreateSellAsync_MissingDistributor_ThrowsArgumentException()
    {
        _db.Products.Add(new Product { ID = 1, Name = "Pen", Price = 10 });
        await _db.SaveChangesAsync(TestContext.Current.CancellationToken);

        var ex = await Should.ThrowAsync<ArgumentException>(
            () => _sut.CreateSellAsync(new Sell { ProductID = 1, DistributorID = 999 }));
        ex.Message.ShouldContain("Distributor 999");
    }

    [Fact]
    public async Task FilterSoldProducts_NullParameters_ThrowsArgumentNullException()
    {
        await Should.ThrowAsync<ArgumentNullException>(() => _sut.FilterSoldProducts(null!));
    }

    [Fact]
    public async Task FilterSoldProducts_ByProductId_ReturnsOnlyMatchingSells()
    {
        _db.Sells.AddRange(
            new Sell { ID = 1, ProductID = 1, DistributorID = 1 },
            new Sell { ID = 2, ProductID = 2, DistributorID = 1 });
        await _db.SaveChangesAsync(TestContext.Current.CancellationToken);

        var result = await _sut.FilterSoldProducts(new SellResourceParameters { ProductID = 2 });

        var only = result.ShouldHaveSingleItem();
        only.ID.ShouldBe(2);
    }

    [Fact]
    public async Task FilterSoldProducts_NoFilters_ReturnsAllSells()
    {
        _db.Sells.AddRange(
            new Sell { ID = 1, ProductID = 1, DistributorID = 1 },
            new Sell { ID = 2, ProductID = 2, DistributorID = 2 });
        await _db.SaveChangesAsync(TestContext.Current.CancellationToken);

        var result = await _sut.FilterSoldProducts(new SellResourceParameters());

        result.Count.ShouldBe(2);
    }
}
