using MarketerSystem.Abstractions.Repository;
using MarketerSystem.Domain.Model;

namespace MarketerSystem.Repository.Repository;

public class AddressRepository(IUnitOfWork context) : RepositoryBase<Address>(context), IAddressRepository;
