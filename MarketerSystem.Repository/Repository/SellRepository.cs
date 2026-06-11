using MarketerSystem.Abstractions.Repository;
using MarketerSystem.Domain.Model;

namespace MarketerSystem.Repository.Repository;

public class SellRepository(IUnitOfWork context) : RepositoryBase<Sell>(context), ISellRepository;
