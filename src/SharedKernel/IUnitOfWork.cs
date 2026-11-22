namespace SharedKernel;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken token);
}