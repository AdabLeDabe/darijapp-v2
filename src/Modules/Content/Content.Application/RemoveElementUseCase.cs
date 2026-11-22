using SharedKernel;

namespace Content.Application;

public class RemoveElementUseCase(ITestElementRepository repository) : IUseCase<int, VoidOutput>
{
    public async Task<Result<VoidOutput>> ExecuteAsync(int input)
    {
        await repository.RemoveElement(input);

        return Result<VoidOutput>.Success(new());
    }
}