using Mapster;
using MarketerSystem.Common.DTO;
using MarketerSystem.Domain.Model;

namespace MarketerSysytem.Web.Profiles;

public class BonusPaymentProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<BonusPayment, BonusPaymentDTO>();
    }
}
