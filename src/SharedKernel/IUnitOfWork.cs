namespace Content.Application;

public interface IUnitOfWork
{
    void SaveChanges();

    Task SaveChangesAsync();
}