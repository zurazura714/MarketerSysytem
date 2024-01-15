using AutoMapper;
using MarketerSystem.Common.DTO;
using MarketerSystem.Domain.Model;

namespace MarketerSysytem.Web.Profiles
{
    public partial class PictureProfile
    {
        public class ProductProfile : Profile
        {
            public ProductProfile()
            {
                CreateMap<Product, ProductDTO>();
            }
        }
    }
}