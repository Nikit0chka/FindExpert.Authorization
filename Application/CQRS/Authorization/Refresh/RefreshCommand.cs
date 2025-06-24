using Ardalis.SharedKernel;
using Domain.Utils;

namespace Application.CQRS.Authorization.Refresh;

public readonly record struct RefreshCommand(string RefreshToken):ICommand<OperationResult<RefreshCommandResult>>;