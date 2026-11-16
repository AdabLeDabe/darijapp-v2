namespace Content.Application;

public interface IUnitOfWork
{
    void SaveChanges();
}