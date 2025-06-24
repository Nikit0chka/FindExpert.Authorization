using Domain.AggregateModels;

namespace API.Endpoints.Login;

public readonly record struct LoginRequest(string Email, string Password, Role Role);