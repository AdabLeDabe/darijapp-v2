using Content.Domain;
using SharedKernel;

namespace Content.Application;

public class GetElementsUseCase(ITestElementRepository repository) : IUseCase<VoidInput, string>
{
    public Result<string> Execute(VoidInput _)
    {
        var output = String.Join(' ', repository.GetElements());
        return Result<string>.Success(output);
    }
}