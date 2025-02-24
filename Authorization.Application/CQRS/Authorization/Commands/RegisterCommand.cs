using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Authorization.Application.CQRS.Authorization.Commands;

public sealed record RegisterCommand(string Login, string Password):ICommand<Result>;