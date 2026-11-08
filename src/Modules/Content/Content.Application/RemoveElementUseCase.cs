using SharedKernel;

namespace Content.Application;

public class RemoveElementUseCase(ITestElementRepository repository) : IUseCase<int, VoidOutput>
{
    public Result<VoidOutput> Execute(int input)
    {
        repository.RemoveElement(input);

        return Result<VoidOutput>.Success(new());
    }
}