using MarketerSystem.Abstractions.Repository;
using MarketerSystem.Domain.Model;

namespace MarketerSystem.Repository.Repository;

public class DistributorRepository(IUnitOfWork context) : RepositoryBase<Distributor>(context), IDistributorRepository;
