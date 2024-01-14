using MarketerSystem.Abstractions.Repository;
using MarketerSystem.Abstractions.Service;
using MarketerSystem.Domain.Model;

namespace MarketerSystem.Service.Service
{
    public class PictureService : ServiceBase<Picture, IPictureRepository>, IPictureService
    {
        public PictureService(IUnitOfWork context, IPictureRepository pictureRepository) : base(context, pictureRepository)
        {
        }
    }
}
