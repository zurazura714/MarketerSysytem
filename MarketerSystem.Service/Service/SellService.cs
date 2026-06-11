using MarketerSystem.Abstractions.Repository;
using MarketerSystem.Abstractions.Service;
using MarketerSystem.Domain.Model;
using MarketerSystem.Domain.ResourceParameters;

namespace MarketerSystem.Service.Service;

public class SellService(IUnitOfWork context, ISellRepository sellRepository)
    : ServiceBase<Sell, ISellRepository>(context, sellRepository), ISellService
{
    public async Task<List<Sell>> FilterSoldProducts(SellResourceParameters parameters)
    {
        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        var soldProducts = (await SetAsync())
            .Where(a =>
                (parameters.ProductID == null || a.ProductID == parameters.ProductID) &&
                (parameters.DistributorID == null || a.DistributorID == parameters.DistributorID) &&
                (parameters.SoldDate == null || a.SoldDate == parameters.SoldDate))
            .ToList();

        return soldProducts;
    }
}
