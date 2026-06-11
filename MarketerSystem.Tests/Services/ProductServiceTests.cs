using MarketerSystem.Abstractions.Repository;
using MarketerSystem.Domain.Model;
using MarketerSystem.Repository.Repository;
using MarketerSystem.Service.Service;
using MarketerSystem.Tests.Infrastructure;
using Moq;
using Shouldly;

namespace MarketerSystem.Tests.Services;

/// <summary>
/// Mock-based tests pin the ServiceBase orchestration contract
/// (repository call + unit-of-work commit); ListAsync runs on EF InMemory
/// because it materializes a real queryable.
/// </summary>
public class ProductServiceTests
{
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<IProductRepository> _repo = new();
    private readonly ProductService _sut;

    public ProductServiceTests()
    {
        _sut = new ProductService(_uow.Object, _repo.Object);
    }

    [Fact]
    public async Task FetchAsync_ExistingId_ReturnsProductFromRepository()
    {
        var product = new Product { ID = 1, Name = "Pen", Price = 10 };
        _repo.Setup(r => r.FetchAsync(1)).ReturnsAsync(product);

        var result = await _sut.FetchAsync(1);

        result.ShouldBeSameAs(product);
    }

    [Fact]
    public async Task ListAsync_ReturnsAllPersistedProducts()
    {
        using var db = InMemoryContextFactory.Create();
        var service = new ProductService(db, new ProductRepository(db));
        db.Products.AddRange(
            new Product { ID = 1, Name = "Pen", Price = 10 },
            new Product { ID = 2, Name = "Notebook", Price = 25 });
        await db.SaveChangesAsync(TestContext.Current.CancellationToken);

        var result = await service.ListAsync();

        result.Count.ShouldBe(2);
    }

    [Fact]
    public async Task SaveAsync_DelegatesToRepositoryAndCommitsUnit()
    {
        var product = new Product { ID = 0, Name = "Marker", Price = 5 };

        await _sut.SaveAsync(product);

        _repo.Verify(r => r.SaveAsync(product), Times.Once);
        _uow.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ByEntity_RemovesAndCommits()
    {
        var product = new Product { ID = 7, Name = "Stapler", Price = 12 };

        await _sut.DeleteAsync(product);

        _repo.Verify(r => r.DeleteAsync(product), Times.Once);
        _uow.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ById_WhenEntityExists_DeletesAndCommits()
    {
        var product = new Product { ID = 1, Name = "Pen", Price = 10 };
        _repo.Setup(r => r.FetchAsync(1)).ReturnsAsync(product);

        await _sut.DeleteAsync(1);

        _repo.Verify(r => r.DeleteAsync(product), Times.Once);
        _uow.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ById_WhenEntityMissing_DoesNotCommit()
    {
        _repo.Setup(r => r.FetchAsync(404)).ReturnsAsync((Product)null!);

        await _sut.DeleteAsync(404);

        _repo.Verify(r => r.DeleteAsync(It.IsAny<Product>()), Times.Never);
        _uow.Verify(u => u.CommitAsync(), Times.Never);
    }
}
