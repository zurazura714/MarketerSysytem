using MarketerSystem.Abstractions.Repository;
using MarketerSystem.Abstractions.Service;
using MarketerSystem.Domain.Model;

namespace MarketerSystem.Service.Service;

public class PassportService(IUnitOfWork context, IPassportRepository passportRepository)
    : ServiceBase<Passport, IPassportRepository>(context, passportRepository), IPassportService;
