using Mapster;
using MarketerSystem.Common.DTO;
using MarketerSystem.Domain.Model;

namespace MarketerSysytem.Web.Profiles
{
    public partial class SellProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Sell, SellDTO>();
            config.NewConfig<Sell, SellCreateDTO>().TwoWays();
        }
    }
    public partial class BonusPaymentProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<BonusPayment, BonusPaymentDTO>();
        }
    }
}
