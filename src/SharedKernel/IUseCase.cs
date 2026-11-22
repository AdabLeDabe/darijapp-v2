namespace SharedKernel;

public interface IUseCase<in TInput, TOutput>
{
    Task<Result<TOutput>> ExecuteAsync(TInput input, CancellationToken token);
}