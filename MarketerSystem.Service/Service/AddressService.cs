using MarketerSystem.Abstractions.Repository;
using MarketerSystem.Abstractions.Service;
using MarketerSystem.Domain.Model;

namespace MarketerSystem.Service.Service;

public class AddressService(IUnitOfWork context, IAddressRepository addressRepository)
    : ServiceBase<Address, IAddressRepository>(context, addressRepository), IAddressService;
