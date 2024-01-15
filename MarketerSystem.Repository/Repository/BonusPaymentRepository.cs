using MarketerSystem.Abstractions.Repository;
using MarketerSystem.Domain.Model;

namespace MarketerSystem.Repository.Repository
{
    public class BonusPaymentRepository : RepositoryBase<BonusPayment>, IBonusPaymentRepository
    {
        public BonusPaymentRepository(IUnitOfWork context) : base(context)
        {
        }
    }
}
