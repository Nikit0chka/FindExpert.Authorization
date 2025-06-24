namespace Application.CQRS.Authorization.Refresh;

public readonly record struct RefreshCommandResult(string AccessToken);