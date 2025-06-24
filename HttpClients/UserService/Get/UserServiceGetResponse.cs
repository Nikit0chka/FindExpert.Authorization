using Domain.AggregateModels;

namespace HttpClients.UserService.Get;

public record UserServiceGetResponse(int UserId, List<Role> Roles);