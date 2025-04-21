namespace Infrastructure.Repositories;

public interface IRepository<T> where T : class
{
    Task AddAsync(T entity);
    Task<T?> GetByIdAsync(Guid id);
    IQueryable<T> Query();
}