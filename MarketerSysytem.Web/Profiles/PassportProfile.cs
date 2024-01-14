using AutoMapper;
using MarketerSystem.Common.DTO;
using MarketerSystem.Domain.Model;

namespace MarketerSysytem.Web.Profiles
{
    public class PassportProfile : Profile
    {
        public PassportProfile()
        {
            CreateMap<Passport, PassportDTO>();
            CreateMap<Passport, PassportCreateDTO>().ReverseMap();
        }
    }
}