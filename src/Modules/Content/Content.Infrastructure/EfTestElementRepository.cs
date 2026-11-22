using Content.Application;
using Content.Domain;
using Microsoft.EntityFrameworkCore;

namespace Content.Infrastructure;

public class EfTestElementRepository(TestDbContext dbContext) : ITestElementRepository
{
    public Task AddElement(TestElement element, CancellationToken token)
    {
        dbContext.TestElements.Add(element);

        return Task.CompletedTask;
    }

    public async Task RemoveElement(int id, CancellationToken token)
    {
        var element = await dbContext.TestElements.FirstOrDefaultAsync(e => e.Id == id, token);

        if (element != null)
        {
            dbContext.TestElements.Remove(element);
        }
    }

    public async Task<IReadOnlyList<TestElement>> GetElements(CancellationToken token)
    {
        return await dbContext.TestElements.ToListAsync(token);
    }
}