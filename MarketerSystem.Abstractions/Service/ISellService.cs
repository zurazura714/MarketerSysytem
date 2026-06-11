using MarketerSystem.Domain.Model;
using MarketerSystem.Domain.ResourceParameters;

namespace MarketerSystem.Abstractions.Service;

public interface ISellService : IServiceBase<Sell>
{
    Task<List<Sell>> FilterSoldProducts(SellResourceParameters parameters);

    /// <summary>
    /// Validates the referenced product and distributor exist, snapshots the price
    /// from the product, and persists. Throws <see cref="ArgumentException"/> when
    /// either reference is invalid.
    /// </summary>
    Task<Sell> CreateSellAsync(Sell sell);
}
