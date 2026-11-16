using Content.Application;

namespace Content.Infrastructure;

public class EFUnitOfWork(TestDbContext db) : IUnitOfWork
{
    public void SaveChanges()
    {
        db.SaveChanges();
    }
}