using FluentAssertions;
using MarketerSystem.Abstractions.Repository;
using MarketerSystem.Domain.Model;
using MarketerSystem.Service.Service;
using Moq;

namespace MarketerSystem.Tests.Services;

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

        result.Should().BeSameAs(product);
    }

    [Fact]
    public async Task SetAsync_ReturnsAllProductsFromRepository()
    {
        var products = new List<Product>
        {
            new() { ID = 1, Name = "Pen", Price = 10 },
            new() { ID = 2, Name = "Notebook", Price = 25 }
        };
        _repo.Setup(r => r.SetAsync()).ReturnsAsync(products);

        var result = await _sut.SetAsync();

        result.Should().BeEquivalentTo(products);
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
}
