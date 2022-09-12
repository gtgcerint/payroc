using URLApi.DAL.Repos;

namespace URLApi.DAL
{
    public interface IUnitOfWork
    {
        IUrlRepository Urls { get; }
    }
}
