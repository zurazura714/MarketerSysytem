using MarketerSystem.Abstractions.Repository;
using MarketerSystem.Abstractions.Service;
using MarketerSystem.Domain.Model;

namespace MarketerSystem.Service.Service;

public class ContactInfoService(IUnitOfWork context, IContactInfoRepository contactInfoRepository)
    : ServiceBase<ContactInfo, IContactInfoRepository>(context, contactInfoRepository), IContactInfoService;
