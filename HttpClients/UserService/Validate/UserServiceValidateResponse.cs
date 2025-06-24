using Domain.AggregateModels;

namespace HttpClients.UserService.Validate;

public sealed record UserServiceValidateResponse(int UserId, List<Role> Roles);