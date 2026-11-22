using Content.Application;
using SharedKernel;

namespace Content.Infrastructure;

public class EFUnitOfWork(TestDbContext db) : IUnitOfWork
{
    public void SaveChanges()
    {
        db.SaveChanges();
    }

    public Task SaveChangesAsync()
    {
        return db.SaveChangesAsync();
    }
}