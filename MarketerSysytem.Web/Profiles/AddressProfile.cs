using Mapster;
using MarketerSystem.Common.DTO;
using MarketerSystem.Domain.Model;

namespace MarketerSysytem.Web.Profiles;

public class AddressProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Address, AddressDTO>();
        config.NewConfig<Address, AddressCreateDTO>().TwoWays();
    }
}
