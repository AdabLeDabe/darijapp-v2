namespace Identity.Application.Login;

public sealed record AccessToken(string Value, DateTimeOffset ExpiresAt);