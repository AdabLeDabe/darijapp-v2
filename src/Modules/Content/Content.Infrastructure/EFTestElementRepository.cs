using Content.Application;
using Content.Domain;

namespace Content.Infrastructure;

public class EFTestElementRepository(TestDbContext dbContext) : ITestElementRepository
{
    public async Task AddElement(TestElement element)
    {
        await dbContext.TestElements.AddAsync(element);
    }

    public Task RemoveElement(int id)
    {
        var element = dbContext.TestElements.FirstOrDefault(e => e.Id == id);

        if (element != null)
        {
            dbContext.TestElements.Remove(element);
        }

        return Task.CompletedTask;
    }

    public async Task<IQueryable<TestElement>> GetElements()
    {
        return dbContext.TestElements.AsQueryable();
    }
}