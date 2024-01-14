using AutoMapper;
using MarketerSystem.Common.DTO;
using MarketerSystem.Domain.Model;

namespace MarketerSysytem.Web.Profiles
{
    public class DistributorProfile : Profile
    {
        public DistributorProfile()
        {
            CreateMap<Distributor, DistributorDTO>();
            CreateMap<Distributor, DistributorCreateDTO>().ReverseMap();
        }
    }
}