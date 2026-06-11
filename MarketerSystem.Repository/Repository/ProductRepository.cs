using MarketerSystem.Abstractions.Repository;
using MarketerSystem.Domain.Model;

namespace MarketerSystem.Repository.Repository;

public class ProductRepository(IUnitOfWork context) : RepositoryBase<Product>(context), IProductRepository;
