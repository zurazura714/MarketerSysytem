using Mapster;
using MarketerSystem.Common.DTO;
using MarketerSystem.Domain.Model;

namespace MarketerSysytem.Web.Profiles;

public class DistributorProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Distributor, DistributorDTO>();
        config.NewConfig<Distributor, DistributorCreateDTO>().TwoWays();
    }
}
