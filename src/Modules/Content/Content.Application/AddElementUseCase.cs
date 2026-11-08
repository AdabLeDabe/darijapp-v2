using Content.Domain;
using SharedKernel;

namespace Content.Application;

public sealed class AddElementUseCase(ITestElementRepository repository) : IUseCase<AddElementInput, VoidOutput>
{
    public Result<VoidOutput> Execute(AddElementInput input)
    {
        var testElement = TestElement.Create(input.Content);

        if (testElement is { Status: ResultStatus.Success, Content: not null })
        {
            repository.AddElement(testElement.Content);
            return Result<VoidOutput>.Success(new());
        }

        return new Result<VoidOutput>() { Status = testElement.Status };
    }
}