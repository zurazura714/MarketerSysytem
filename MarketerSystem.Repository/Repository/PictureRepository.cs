using MarketerSystem.Abstractions.Repository;
using MarketerSystem.Domain.Model;

namespace MarketerSystem.Repository.Repository
{
    public class PictureRepository : RepositoryBase<Picture>, IPictureRepository
    {
        public PictureRepository(IUnitOfWork context) : base(context)
        {
        }
    }
}
