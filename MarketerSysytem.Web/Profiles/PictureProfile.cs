using Mapster;
using MarketerSystem.Common.DTO;
using MarketerSystem.Domain.Model;

namespace MarketerSysytem.Web.Profiles
{
    public partial class PictureProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Picture, PictureDTO>();
            config.NewConfig<Picture, PictureCreateDTO>().TwoWays();
        }
    }
}
