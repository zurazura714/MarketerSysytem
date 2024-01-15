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
            CreateMap<Sell, SellCreateDTO>().ReverseMap();
        }
    }
    public partial class BonusPaymentProfile : Profile
    {
        public BonusPaymentProfile()
        {
            CreateMap<BonusPayment, BonusPaymentDTO>();
        }
    }
}