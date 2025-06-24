namespace Application.CQRS.Authorization.Login;

public readonly record struct LoginCommandResult(string AccessToken, string RefreshToken);