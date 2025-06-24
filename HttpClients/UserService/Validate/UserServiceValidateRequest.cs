using Domain.AggregateModels;

namespace HttpClients.UserService.Validate;

public sealed record UserServiceValidateRequest(string Email, string Password, Role Role);