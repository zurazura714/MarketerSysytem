using MarketerSystem.Abstractions.Repository;
using MarketerSystem.Abstractions.Service;
using MarketerSystem.Domain.Model;

namespace MarketerSystem.Service.Service;

public class DistributorService(IUnitOfWork context, IDistributorRepository distributorRepository)
    : ServiceBase<Distributor, IDistributorRepository>(context, distributorRepository), IDistributorService;
