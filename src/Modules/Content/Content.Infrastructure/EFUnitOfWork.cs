using Content.Application;
using SharedKernel;

namespace Content.Infrastructure;

public class EFUnitOfWork(TestDbContext db) : IUnitOfWork
{
    public Task SaveChangesAsync(CancellationToken token)
    {
        return db.SaveChangesAsync(token);
    }
}