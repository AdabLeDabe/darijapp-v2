using Content.Application;
using SharedKernel;

namespace Api;

public static class ServiceExtensions
{
    public static void AddWriteUseCase<TClass, TInput, TOutput>(this IServiceCollection serviceCollection)
        where TClass : class, IUseCase<TInput, TOutput>
    {
        serviceCollection.AddScoped<TClass>();
        serviceCollection.AddScoped<IUseCase<TInput, TOutput>>(sp =>
            new Decorator<TInput, TOutput>(
                sp.GetRequiredService<IUnitOfWork>(),
                sp.GetRequiredService<TClass>()));
    }
}