using SharedKernel;

namespace Content.Application;

public class RemoveElementUseCase(ITestElementRepository repository) : IUseCase<int, VoidOutput>
{
    public async Task<Result<VoidOutput>> ExecuteAsync(int input, CancellationToken token)
    {
        await repository.RemoveElement(input, token);

        return Result<VoidOutput>.Success(new());
    }
}