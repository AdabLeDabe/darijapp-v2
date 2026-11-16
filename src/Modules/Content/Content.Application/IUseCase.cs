using SharedKernel;

namespace Content.Application;

public interface IUseCase<in TInput, TOutput>
{
    Result<TOutput> Execute(TInput input);
}