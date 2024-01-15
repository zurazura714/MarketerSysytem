using AutoMapper;
using MarketerSystem.Common.DTO;
using MarketerSystem.Domain.Model;

namespace MarketerSysytem.Web.Profiles
{
    public partial class PictureProfile : Profile
    {
        public PictureProfile()
        {
            CreateMap<Picture, PictureDTO>();
            CreateMap<Picture, PictureCreateDTO>().ReverseMap();
        }
    }
}