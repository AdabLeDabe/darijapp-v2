using Content.Application;
using Content.Domain;

namespace Content.Infrastructure;

public class EFTestElementRepository(TestDbContext dbContext) : ITestElementRepository
{
    public void AddElement(TestElement element)
    {
        dbContext.TestElements.Add(element);
    }

    public void RemoveElement(int id)
    {
        var element = dbContext.TestElements.FirstOrDefault(e => e.Id == id);

        if (element != null)
        {
            dbContext.TestElements.Remove(element);
        }
    }

    public IQueryable<TestElement> GetElements()
    {
        return dbContext.TestElements.AsQueryable();
    }
}