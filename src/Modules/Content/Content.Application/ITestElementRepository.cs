using Content.Domain;

namespace Content.Application;

public interface ITestElementRepository
{
    void AddElement(TestElement element);
    void RemoveElement(int id);
    IQueryable<TestElement> GetElements();
}