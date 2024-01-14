using AutoMapper;
using MarketerSystem.Common.DTO;
using MarketerSystem.Domain.Model;

namespace MarketerSysytem.Web.Profiles
{
    public class ContactInfoProfile : Profile
    {
        public ContactInfoProfile()
        {
            CreateMap<ContactInfo, ContactInfoDTO>();
            CreateMap<ContactInfo, ContactInfoCreateDTO>().ReverseMap();
        }
    }
}