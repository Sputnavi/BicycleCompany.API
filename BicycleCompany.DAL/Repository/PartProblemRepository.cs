using BicycleCompany.DAL.Contracts;
using BicycleCompany.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BicycleCompany.DAL.Repository
{
    public class PartProblemRepository : RepositoryBase<PartProblem>, IPartProblemRepository
    {
        public PartProblemRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {

        }
        //private Task Get

        public Task CreatePartProblemAsync(Guid clientId, Guid id, PartProblem partProblem)
        {
            throw new NotImplementedException();
        }

        public Task DeletePartProblemAsync(PartProblem partProblem)
        {
            throw new NotImplementedException();
        }

        public Task<PartProblem> GetPartProblemAsync(Guid clientId, Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PartProblem>> GetPartProblemsAsync(Guid clientId, Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
