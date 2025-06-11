using CoreXCrud.Entities;

namespace CoreXCrud.Repositories
{
    /// <summary>
    /// Unit of Work Interface - Tüm Repository'leri tek bir yapıdan yönetmek için.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> Users { get; }
        IRepository<Product> Products { get; }
        IRepository<Order> Orders { get; }
        IRepository<OrderDetail> OrderDetails { get; }
        Task<int> CompleteAsync();
    }
}
