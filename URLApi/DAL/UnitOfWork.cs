using URLApi.DAL.Repos;

namespace URLApi.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(IUrlRepository productRepository)
        {
            Urls = productRepository;
        }

        public IUrlRepository Urls { get; }
        
    }
}
