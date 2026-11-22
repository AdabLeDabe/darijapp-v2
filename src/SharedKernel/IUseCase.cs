using SharedKernel;

namespace Content.Application;

public interface IUseCase<in TInput, TOutput>
{
    Task<Result<TOutput>> ExecuteAsync(TInput input, CancellationToken token);
}