using MarketerSystem.Abstractions.Repository;
using MarketerSystem.Abstractions.Service;
using MarketerSystem.Domain.Model;
using MarketerSystem.Domain.ResourceParameters;
using Microsoft.EntityFrameworkCore;

namespace MarketerSystem.Service.Service;

public class SellService(
    IUnitOfWork context,
    ISellRepository sellRepository,
    IProductRepository productRepository,
    IDistributorRepository distributorRepository)
    : ServiceBase<Sell, ISellRepository>(context, sellRepository), ISellService
{
    public Task<List<Sell>> FilterSoldProducts(SellResourceParameters parameters)
    {
        ArgumentNullException.ThrowIfNull(parameters);

        return _repository.Set()
            .Where(a =>
                (parameters.ProductID == null || a.ProductID == parameters.ProductID) &&
                (parameters.DistributorID == null || a.DistributorID == parameters.DistributorID) &&
                (parameters.SoldDate == null || a.SoldDate == parameters.SoldDate))
            .ToListAsync();
    }

    public async Task<Sell> CreateSellAsync(Sell sell)
    {
        ArgumentNullException.ThrowIfNull(sell);

        var product = await productRepository.FetchAsync(sell.ProductID)
            ?? throw new ArgumentException($"Product {sell.ProductID} not found", nameof(sell));
        var distributor = await distributorRepository.FetchAsync(sell.DistributorID)
            ?? throw new ArgumentException($"Distributor {sell.DistributorID} not found", nameof(sell));

        sell.Product = product;
        sell.Distributor = distributor;
        sell.ProductPrice = product.Price;
        sell.ProductUnitPrice = product.Price;
        sell.ProductTotalPrice = product.Price;

        await SaveAsync(sell);
        return sell;
    }
}
