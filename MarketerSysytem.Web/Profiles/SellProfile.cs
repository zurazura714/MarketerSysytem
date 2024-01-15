using AutoMapper;
using MarketerSystem.Common.DTO;
using MarketerSystem.Domain.Model;

namespace MarketerSysytem.Web.Profiles
{
    public partial class SellProfile : Profile
    {
        public SellProfile()
        {
            CreateMap<Sell, SellDTO>();
        }
    }
}