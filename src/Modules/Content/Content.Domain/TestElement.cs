using SharedKernel;

namespace Content.Domain;

public sealed class TestElement
{
    private TestElement()
    {
        
    }

    public static Result<TestElement> Create(string content)
    {
        if (content == "hello")
        {
            return Result<TestElement>.Forbidden();
        }
        return Result<TestElement>.Success(new TestElement() { Content = content });
    }
    
    public int Id { get; private set; }
    
    public string Content { get; private set; }
}