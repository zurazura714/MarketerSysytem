using MarketerSystem.Abstractions.Repository;
using MarketerSystem.Abstractions.Service;
using MarketerSystem.Domain.Model;

namespace MarketerSystem.Service.Service
{
    public class ContactInfoService : ServiceBase<ContactInfo, IContactInfoRepository>, IContactInfoService
    {
        public ContactInfoService(IUnitOfWork context, IContactInfoRepository contactInfoRepository) : base(context, contactInfoRepository)
        {
        }
    }
}
