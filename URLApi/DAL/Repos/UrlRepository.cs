using Dapper;
using System.Data.SqlClient;
using URLApi.DAL.Models;

namespace URLApi.DAL.Repos
{
    public class UrlRepository : IUrlRepository
    {
        private readonly IConfiguration configuration;
        public UrlRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<int> AddAsync(UrlMap entity)
        {
            var sql = "INSERT INTO UrlMap (Hash, LongUrl) OUTPUT INSERTED.Id VALUES (@Hash, @LongUrl)";
            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = connection.ExecuteScalar<int>(sql, entity);
                return result;
            }
        }

        public async Task<UrlMap> GetByHashAsync(string Hash)
        {
            var sql = "SELECT * FROM UrlMap WHERE Hash = @Hash";
            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.QuerySingleOrDefaultAsync<UrlMap>(sql, new { Hash = Hash });
                return result;
            }
        }

        public async Task<UrlMap> GetByChunkAsync(string Chunk)
        {
            var sql = "SELECT * FROM UrlMap WHERE Chunk = @Chunk";
            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.QuerySingleOrDefaultAsync<UrlMap>(sql, new { Chunk = Chunk });
                return result;
            }
        }

        public async Task<int> UpdateAsync(UrlMap entity)
        {
            var sql = "UPDATE UrlMap SET Chunk = @Chunk WHERE Id = @Id";
            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql, entity);
                return result;
            }
        }
    }
}
