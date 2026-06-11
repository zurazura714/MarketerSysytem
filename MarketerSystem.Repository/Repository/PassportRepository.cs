using MarketerSystem.Abstractions.Repository;
using MarketerSystem.Domain.Model;

namespace MarketerSystem.Repository.Repository;

public class PassportRepository(IUnitOfWork context) : RepositoryBase<Passport>(context), IPassportRepository;
