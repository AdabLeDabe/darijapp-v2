using Content.Domain;

namespace Content.Application;

public interface ITestElementRepository
{
    Task AddElement(TestElement element, CancellationToken? token = null);
    Task RemoveElement(int id, CancellationToken? token = null);
    Task<IReadOnlyList<TestElement>> GetElements(CancellationToken? token = null);
}