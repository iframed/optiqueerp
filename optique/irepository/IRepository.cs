using Ardalis.Specification;

using Ardalis.Specification.EntityFrameworkCore;
namespace MyAspNetApp.Repositories
{
    public interface IRepository<T> : IRepositoryBase<T>, IReadRepositoryBase<T> where T : class
    {


    //Task<T?> GetByIdAsync(int id);
     Task<T?> GetByIdAsync(int id, Func<IQueryable<T>, IQueryable<T>>? include = null);
    Task<IEnumerable<T>> ListAsync(Func<IQueryable<T>, IQueryable<T>>? include = null, CancellationToken cancellationToken = default);
    Task<T?> GetByNameAsync(string name);
    Task<IEnumerable<T>> ListAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task<IEnumerable<T>> GetAllAsync();



       
    }
}
