using URLApi.DAL.Models;

namespace URLApi.DAL.Repos
{
    public interface IUrlRepository : IGenericRepository<UrlMap>
    {
        Task<UrlMap> GetByHashAsync(string Hash);
        Task<UrlMap> GetByChunkAsync(string Chunk);
    }
}
