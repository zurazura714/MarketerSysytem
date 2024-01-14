using MarketerSystem.Abstractions.Repository;
using MarketerSystem.Abstractions.Service;
using MarketerSystem.Domain.Model;

namespace MarketerSystem.Service.Service
{
    public class AddressService : ServiceBase<Address, IAddressRepository>, IAddressService
    {
        public AddressService(IUnitOfWork context, IAddressRepository addressRepository) : base(context, addressRepository)
        {
        }
    }
}
