using Content.Domain;
using SharedKernel;

namespace Content.Application;

public class GetElementsUseCase(ITestElementRepository repository) : IUseCase<VoidInput, string>
{
    public async Task<Result<string>> ExecuteAsync(VoidInput _)
    {
        var output = String.Join(' ', await repository.GetElements());
        return Result<string>.Success(output);
    }
}