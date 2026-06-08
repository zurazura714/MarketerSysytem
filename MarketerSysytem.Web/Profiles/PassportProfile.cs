using Mapster;
using MarketerSystem.Common.DTO;
using MarketerSystem.Domain.Model;

namespace MarketerSysytem.Web.Profiles
{
    public class PassportProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Passport, PassportDTO>();
            config.NewConfig<Passport, PassportCreateDTO>().TwoWays();
        }
    }
}
