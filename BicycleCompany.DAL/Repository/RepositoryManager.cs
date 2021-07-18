using BicycleCompany.DAL.Contracts;

namespace BicycleCompany.DAL.Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        private RepositoryContext _repositoryContext;
        private IBicycleRepository _bicycleRepository;
        private IClientRepository _clientRepository;
        private IPartRepository _partRepository;
        private IProblemRepository _problemRepository;

        public IBicycleRepository Bicycle
        {
            get
            {
                if (_bicycleRepository is null)
                {
                    _bicycleRepository = new BicycleRepository(_repositoryContext);
                }
                return _bicycleRepository;
            }
        }

        public IClientRepository Client
        {
            get
            {
                if (_clientRepository is null)
                {
                    _clientRepository = new ClientRepository(_repositoryContext);
                }
                return _clientRepository;
            }
        }

        public IPartRepository Part
        {
            get
            {
                if (_partRepository is null)
                {
                    _partRepository = new PartRepository(_repositoryContext);
                }
                return _partRepository;
            }
        }

        public IProblemRepository Problem
        {
            get
            {
                if (_problemRepository is null)
                {
                    _problemRepository = new ProblemRepository(_repositoryContext);
                }
                return _problemRepository;
            }
        }
    }
}
