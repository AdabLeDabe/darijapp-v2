using Microsoft.AspNetCore.Identity;
using SharedKernel;

namespace Identity.Application.Login;

public class LoginUseCase(UserManager<AppUser> userManager, ITokenIssuer tokenIssuer)
    : IUseCase<LoginInput, LoginOutput>
{
    public async Task<Result<LoginOutput>> ExecuteAsync(LoginInput input, CancellationToken token)
    {
        var user = await userManager.FindByEmailAsync(input.Email);
        if (user is null || !await userManager.CheckPasswordAsync(user, input.Password))
        {
            return Result<LoginOutput>.Unauthorized();
        }

        var roles = await userManager.GetRolesAsync(user);
        var accessToken = tokenIssuer.Issue(user, roles.ToArray());
        return Result<LoginOutput>.Success(new LoginOutput(accessToken.Value, accessToken.ExpiresAt));
    }
}