using MarketerSystem.Abstractions.Repository;
using MarketerSystem.Domain.Model;

namespace MarketerSystem.Repository.Repository;

public class BonusPaymentRepository(IUnitOfWork context) : RepositoryBase<BonusPayment>(context), IBonusPaymentRepository;
