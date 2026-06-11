using MarketerSystem.Abstractions.Repository;
using MarketerSystem.Domain.Model;

namespace MarketerSystem.Repository.Repository;

public class ContactInfoRepository(IUnitOfWork context) : RepositoryBase<ContactInfo>(context), IContactInfoRepository;
