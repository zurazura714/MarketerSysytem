using MarketerSystem.Domain.Model;
using MarketerSystem.Domain.ResourceParameters;

namespace MarketerSystem.Abstractions.Service;

public interface IBonusPaymentService : IServiceBase<BonusPayment>
{
    Task<List<BonusPayment>> FilterPaymentsProducts(PaymentFilterParameters parameters);

    /// <summary>Pays out every unpaid sale in the window and returns the payments it created.</summary>
    Task<List<BonusPayment>> GenerateBonusPaymentsForPeriodAsync(PaymentParameters parameters);
}
