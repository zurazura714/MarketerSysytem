using Mapster;
using MarketerSystem.Common.DTO;
using MarketerSystem.Domain.Model;

namespace MarketerSysytem.Web.Profiles;

public class SellProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Sell, SellDTO>();
        config.NewConfig<Sell, SellCreateDTO>().TwoWays();
    }
}
