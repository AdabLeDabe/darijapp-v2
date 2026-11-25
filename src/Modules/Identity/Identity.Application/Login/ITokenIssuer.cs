namespace Identity.Application.Login;

public interface ITokenIssuer
{
    AccessToken Issue(AppUser user, IReadOnlyCollection<string> roles);
}