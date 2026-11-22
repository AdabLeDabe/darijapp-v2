using Microsoft.AspNetCore.Identity;
using SharedKernel;

namespace Identity.Application.Register;

public sealed class RegisterUseCase(UserManager<AppUser> userManager, TimeProvider clock)
    : IUseCase<RegisterInput, Guid>
{
    public async Task<Result<Guid>> ExecuteAsync(RegisterInput input, CancellationToken token)
    {
        var user = new AppUser
        {
            UserName = input.Email,
            Email = input.Email,
            DisplayName = input.DisplayName,
            CreatedAt = clock.GetUtcNow(),
        };
        
        var created = await userManager.CreateAsync(user, input.Password);
        if (!created.Succeeded)
        {
            return Result<Guid>.Invalid();
        }

        var roleAdded = await userManager.AddToRoleAsync(user, Roles.Learner);
        if (!roleAdded.Succeeded)
            throw new InvalidOperationException($"Unable to find role: {Roles.Learner}");
        return Result<Guid>.Success(user.Id);
    }
}