using System.Linq.Expressions;

namespace HirehubWeb.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<Result<IEnumerable<T>>> GetAll();
        Task<Result<bool>> Add(T entity);
        Result<T> GetFirst(Expression<Func<T, bool>> query);
        Task<Result<bool>> Delete(T entity);
        Task<Result<bool>> Update(T entity);
        Task<T> GetFirstAsync(Expression<Func<T, bool>> query);

    }
}
