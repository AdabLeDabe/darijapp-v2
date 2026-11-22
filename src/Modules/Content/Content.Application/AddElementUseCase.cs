using Content.Domain;
using SharedKernel;

namespace Content.Application;

public sealed class AddElementUseCase(ITestElementRepository repository) : IUseCase<AddElementInput, VoidOutput>
{
    public async Task<Result<VoidOutput>> ExecuteAsync(AddElementInput input, CancellationToken token)
    {
        var testElement = TestElement.Create(input.Content);

        if (testElement is { Status: ResultStatus.Success, Content: not null })
        {
            await repository.AddElement(testElement.Content, token);
            return Result<VoidOutput>.Success(new());
        }

        return new Result<VoidOutput>() { Status = testElement.Status };
    }
}