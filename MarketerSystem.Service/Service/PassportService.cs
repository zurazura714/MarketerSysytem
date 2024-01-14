using MarketerSystem.Abstractions.Repository;
using MarketerSystem.Abstractions.Service;
using MarketerSystem.Domain.Model;

namespace MarketerSystem.Service.Service
{
    public class PassportService : ServiceBase<Passport, IPassportRepository>, IPassportService
    {
        public PassportService(IUnitOfWork context, IPassportRepository passportRepository) : base(context, passportRepository)
        {
        }
    }
}
