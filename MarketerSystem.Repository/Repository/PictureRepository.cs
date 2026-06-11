using MarketerSystem.Abstractions.Repository;
using MarketerSystem.Domain.Model;

namespace MarketerSystem.Repository.Repository;

public class PictureRepository(IUnitOfWork context) : RepositoryBase<Picture>(context), IPictureRepository;
