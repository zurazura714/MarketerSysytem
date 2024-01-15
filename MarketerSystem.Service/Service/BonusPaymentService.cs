using MarketerSystem.Abstractions.Repository;
using MarketerSystem.Abstractions.Service;
using MarketerSystem.Domain.Model;
using MarketerSystem.Domain.ResourceParameters;
using Microsoft.EntityFrameworkCore;


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

            var payments = (await SetAsync()).AsQueryable().Include(a => a.Distributor)
                .Where(a =>
                    (parameters.MinPrice == null || parameters.MinPrice <= a.BonusPay) &&
                    (parameters.MaxPrice == null || parameters.MaxPrice >= a.BonusPay)
                    &&
                    (parameters.Name == null || a.Distributor.FirstName.Contains(parameters.Name)) &&
                    (parameters.LastName == null || a.Distributor.LastName.Contains(parameters.LastName))
                    )
                .ToList();

            return payments;
        }
            
    }
}
