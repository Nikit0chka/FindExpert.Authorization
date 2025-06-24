namespace API.Endpoints.Login;

public readonly record struct LoginResponse(string AccessToken, string RefreshToken);