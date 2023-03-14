using Core.Entities;
using DataAccess.Context;
using DataAccess.Repositories.Abstract;

namespace DataAccess.UnitOfWorks
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IRepository<T> GetRepository<T>() where T : class, IEntityBase, new();
        Task<int> SaveAsync();
        int Save();
        ApplicationDbContext GetDbContext();
    }
}
