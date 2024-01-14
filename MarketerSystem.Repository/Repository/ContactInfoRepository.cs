using MarketerSystem.Abstractions.Repository;
using MarketerSystem.Domain.Model;

namespace MarketerSystem.Repository.Repository
{
    public class ContactInfoRepository : RepositoryBase<ContactInfo>, IContactInfoRepository
    {
        public ContactInfoRepository(IUnitOfWork context) : base(context)
        {
        }
    }
}
