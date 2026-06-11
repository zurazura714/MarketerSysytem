using MarketerSystem.Abstractions.Repository;
using MarketerSystem.Abstractions.Service;
using MarketerSystem.Domain.Model;

namespace MarketerSystem.Service.Service;

public class ProductService(IUnitOfWork context, IProductRepository productRepository)
    : ServiceBase<Product, IProductRepository>(context, productRepository), IProductService;
