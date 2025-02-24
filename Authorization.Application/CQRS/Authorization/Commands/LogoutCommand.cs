using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Authorization.Application.CQRS.Authorization.Commands;

public sealed record LogoutCommand(string RefreshToken):ICommand<Result>;