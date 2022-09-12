namespace URLApi.DAL.Repos
{
    public interface IGenericRepository<T> where T : class
    {           
        Task<int> AddAsync(T entity);
        Task<int> UpdateAsync(T entity);
    }
}
