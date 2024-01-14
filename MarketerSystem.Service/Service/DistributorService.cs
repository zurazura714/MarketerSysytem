using MarketerSystem.Abstractions.Repository;
using MarketerSystem.Abstractions.Service;
using MarketerSystem.Domain.Model;
using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketerSystem.Service.Service
{
    public class DistributorService : ServiceBase<Distributor, IDistributorRepository>, IDistributorService
    {
        public DistributorService(IUnitOfWork context, IDistributorRepository distributorRepository) : base(context, distributorRepository)
        {
        }
    }
}
