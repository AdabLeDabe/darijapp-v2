namespace SharedKernel;

public sealed class Decorator<TInput, TOutput>(IUnitOfWork unitOfWork, IUseCase<TInput, TOutput> inner)
    : IUseCase<TInput, TOutput>
{
    public async Task<Result<TOutput>> ExecuteAsync(TInput input, CancellationToken token)
    {
        var result = await inner.ExecuteAsync(input, token);
        if (result.Status == ResultStatus.Success)
        {
            await unitOfWork.SaveChangesAsync();
        }
        
        return result;
    }
}