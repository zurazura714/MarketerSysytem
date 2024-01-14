using MarketerSystem.Abstractions.Repository;
using MarketerSystem.Domain.Model;

namespace MarketerSystem.Repository.Repository
{
    public class AddressRepository : RepositoryBase<Address>, IAddressRepository
    {
        public AddressRepository(IUnitOfWork context) : base(context)
        {
        }
    }
}
