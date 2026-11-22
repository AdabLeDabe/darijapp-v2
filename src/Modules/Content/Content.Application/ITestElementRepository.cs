using Content.Domain;

namespace Content.Application;

public interface ITestElementRepository
{
    Task AddElement(TestElement element);
    Task RemoveElement(int id);
    Task<IQueryable<TestElement>> GetElements();
}