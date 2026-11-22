using Content.Application;

namespace SharedKernel;

public sealed class Decorator<TInput, TOutput>(IUnitOfWork unitOfWork, IUseCase<TInput, TOutput> inner)
    : IUseCase<TInput, TOutput>
{
    public async Task<Result<TOutput>> ExecuteAsync(TInput input)
    {
        var result = await inner.ExecuteAsync(input);
        if (result.Status == ResultStatus.Success)
        {
            await unitOfWork.SaveChangesAsync();
        }
        
        return result;
    }
}