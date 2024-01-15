using MarketerSystem.Domain.Model;
using MarketerSystem.Domain.ResourceParameters;

namespace MarketerSystem.Abstractions.Service
{
    public interface ISellService : IServiceBase<Sell>
    {
        Task<List<Sell>> FilterSoldProducts(SellResourceParameters parameters);
    }
}
