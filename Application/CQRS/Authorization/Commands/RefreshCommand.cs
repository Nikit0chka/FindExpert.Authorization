using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Application.CQRS.Authorization.Commands;

public sealed record RefreshCommand(string RefreshToken):ICommand<Result<string>>;