using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class Repository<T>(AppDbContext context) : IRepository<T>
    where T : class
{
    public Task AddAsync(T entity)
    {
        context.Set<T>().Add(entity);
        return Task.CompletedTask;
    }

    public Task<T?> GetByIdAsync(Guid id)
        => context.Set<T>().FindAsync(id).AsTask();

    public IQueryable<T> Query() => context.Set<T>();
}