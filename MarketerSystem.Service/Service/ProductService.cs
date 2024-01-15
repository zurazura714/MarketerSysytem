using MarketerSystem.Abstractions.Repository;
using MarketerSystem.Abstractions.Service;
using MarketerSystem.Domain.Model;

namespace MarketerSystem.Service.Service
{
    public class ProductService : ServiceBase<Product, IProductRepository>, IProductService
    {
        public ProductService(IUnitOfWork context, IProductRepository productRepository) : base(context, productRepository)
        {
        }
    }
}
