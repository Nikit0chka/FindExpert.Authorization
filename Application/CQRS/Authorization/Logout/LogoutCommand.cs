using Ardalis.SharedKernel;
using Domain.Utils;

namespace Application.CQRS.Authorization.Logout;

public readonly record struct LogoutCommand(string RefreshToken):ICommand<OperationResult>;