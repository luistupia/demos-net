using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public IRepository<T> Repository<T>() where T : class
        => new Repository<T>(context);

    public Task<int> SaveChangesAsync() => context.SaveChangesAsync();
}