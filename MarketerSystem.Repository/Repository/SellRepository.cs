using MarketerSystem.Abstractions.Repository;
using MarketerSystem.Domain.Model;

namespace MarketerSystem.Repository.Repository
{
    public class SellRepository : RepositoryBase<Sell>, ISellRepository
    {
        public SellRepository(IUnitOfWork context) : base(context)
        {
        }
    }
}
