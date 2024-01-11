using MarketerSystem.Abstractions.Repository;
using MarketerSystem.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketerSystem.Repository.Repository
{
    public class DistributorRepository : RepositoryBase<Distributor>, IDistributorRepository
    {
        public DistributorRepository(IUnitOfWork context) : base(context)
        {
        }
    }
}
