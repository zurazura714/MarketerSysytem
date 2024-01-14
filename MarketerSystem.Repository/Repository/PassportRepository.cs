using MarketerSystem.Abstractions.Repository;
using MarketerSystem.Domain.Model;

namespace MarketerSystem.Repository.Repository
{
    public class PassportRepository : RepositoryBase<Passport>, IPassportRepository
    {
        public PassportRepository(IUnitOfWork context) : base(context)
        {
        }
    }
}
