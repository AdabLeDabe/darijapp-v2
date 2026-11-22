using Content.Domain;
using SharedKernel;

namespace Content.Application;

public class GetElementsUseCase(ITestElementRepository repository) : IUseCase<VoidInput, string>
{
    public async Task<Result<string>> ExecuteAsync(VoidInput _, CancellationToken token)
    {
        var output = String.Join(' ', await repository.GetElements(token));
        return Result<string>.Success(output);
    }
}