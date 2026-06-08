using Mapster;
using MarketerSystem.Common.DTO;
using MarketerSystem.Domain.Model;

namespace MarketerSysytem.Web.Profiles
{
    public class ContactInfoProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<ContactInfo, ContactInfoDTO>();
            config.NewConfig<ContactInfo, ContactInfoCreateDTO>().TwoWays();
        }
    }
}
