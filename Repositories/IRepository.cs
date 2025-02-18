using System.Linq.Expressions;

namespace CoreXCrud.Repositories
{
    /// <summary>
    /// Generic Repository Interface - Tüm veri erişim işlemleri için ortak interface.
    /// </summary>
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }
}
