using Domain.Utils;
using HttpClients.UserService.Get;
using HttpClients.UserService.Validate;
using UserServiceValidateRequest = HttpClients.UserService.Validate.UserServiceValidateRequest;

namespace HttpClients.Contracts;

public interface IUserServiceClient
{
    Task<OperationResult<UserServiceValidateResponse>> ValidateUserAsync(UserServiceValidateRequest request, CancellationToken cancellationToken);
    Task<OperationResult<UserServiceGetResponse>> GetUserAsync(UserServiceGetRequest request, CancellationToken cancellationToken);
}