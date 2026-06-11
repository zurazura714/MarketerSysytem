using MarketerSystem.Abstractions.Repository;
using MarketerSystem.Abstractions.Service;
using MarketerSystem.Domain.Model;

namespace MarketerSystem.Service.Service;

public class PictureService(IUnitOfWork context, IPictureRepository pictureRepository)
    : ServiceBase<Picture, IPictureRepository>(context, pictureRepository), IPictureService;
