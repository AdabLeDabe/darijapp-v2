namespace SharedKernel;

public interface IUnitOfWork
{
    void SaveChanges();

    Task SaveChangesAsync();
}