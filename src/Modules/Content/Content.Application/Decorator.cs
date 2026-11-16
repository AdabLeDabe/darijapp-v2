using SharedKernel;

namespace Content.Application;

public sealed class Decorator<TInput, TOutput>(IUnitOfWork unitOfWork, IUseCase<TInput, TOutput> inner)
    : IUseCase<TInput, TOutput>
{
    public Result<TOutput> Execute(TInput input)
    {
        var result = inner.Execute(input);
        if (result.Status == ResultStatus.Success)
        {
            unitOfWork.SaveChanges();
        }
        
        return result;
    }
}