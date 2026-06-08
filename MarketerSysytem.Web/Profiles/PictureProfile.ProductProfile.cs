using Mapster;
using MarketerSystem.Common.DTO;
using MarketerSystem.Domain.Model;

namespace MarketerSysytem.Web.Profiles
{
    public partial class PictureProfile
    {
        public class ProductProfile : IRegister
        {
            public void Register(TypeAdapterConfig config)
            {
                config.NewConfig<Product, ProductDTO>();
            }
        }
    }
}
