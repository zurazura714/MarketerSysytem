using MarketerSystem.Abstractions.Repository;
using MarketerSystem.Abstractions.Service;
using MarketerSystem.Common.DTO;
using MarketerSystem.Domain.Model;
using MarketerSystem.Domain.ResourceParameters;
using System;

namespace MarketerSystem.Service.Service
{
    public class SellService : ServiceBase<Sell, ISellRepository>, ISellService
    {
        public SellService(IUnitOfWork context, ISellRepository sellRepository) : base(context, sellRepository)
        {
        }
        public async Task<List<Sell>> FilterSoldProducts(SellResourceParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(SellResourceParameters));
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
}
