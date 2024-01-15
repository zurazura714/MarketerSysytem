using MarketerSystem.Domain.Model;
using MarketerSystem.Domain.ResourceParameters;

namespace MarketerSystem.Abstractions.Service
{
    public interface IBonusPaymentService : IServiceBase<BonusPayment>
    {
        Task<List<BonusPayment>> FilterPaymentsProducts(PaymentFilterParameters parameters);
        Task<List<BonusPayment>> PaymentsDistributors(PaymentFilterParameters parameters);
    }
}
