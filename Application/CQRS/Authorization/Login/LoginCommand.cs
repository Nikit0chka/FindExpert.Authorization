using Ardalis.SharedKernel;
using Domain.AggregateModels;
using Domain.Utils;

namespace Application.CQRS.Authorization.Login;

public readonly record struct LoginCommand(string Email, string Password, Role Role):ICommand<OperationResult<LoginCommandResult>>;