namespace Identity.Application.Login;

public record LoginOutput(string AccessToken, DateTimeOffset ExpiresAt);