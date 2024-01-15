using MarketerSystem.Abstractions.Repository;
using MarketerSystem.Abstractions.Service;
using MarketerSystem.Domain.Model;
using MarketerSystem.Domain.ResourceParameters;

namespace MarketerSystem.Service.Service
{
    public class BonusPaymentService : ServiceBase<BonusPayment, IBonusPaymentRepository>, IBonusPaymentService
    {
        public BonusPaymentService(IUnitOfWork context, IBonusPaymentRepository bonusPaymentRepository) : base(context, bonusPaymentRepository)
        {
        }

        public async Task<List<BonusPayment>> FilterPaymentsProducts(PaymentFilterParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(PaymentParameters));
            }

            var payments = (await SetAsync())
                .Where(a => (parameters.MinPrice <= a.BonusPay ||
                parameters.MaxPrice >= a.BonusPay ||
                parameters.Name.Contains(a.Distributor.FirstName) ||
                parameters.LastName.Contains(a.Distributor.LastName))).ToList();

            return payments;
        }
            
    }
}
