using Content.Domain;

namespace Content.Application;

public interface ITestElementRepository
{
    Task AddElement(TestElement element, CancellationToken token );
    Task RemoveElement(int id, CancellationToken token);
    Task<IReadOnlyList<TestElement>> GetElements(CancellationToken token);
}