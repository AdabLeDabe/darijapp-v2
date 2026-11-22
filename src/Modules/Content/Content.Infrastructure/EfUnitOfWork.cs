using Content.Application;
using SharedKernel;

namespace Content.Infrastructure;

public class EfUnitOfWork(TestDbContext db) : IUnitOfWork
{
    public Task SaveChangesAsync(CancellationToken token)
    {
        return db.SaveChangesAsync(token);
    }
}