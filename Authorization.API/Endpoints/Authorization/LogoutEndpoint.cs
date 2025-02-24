using Authorization.API.Extensions;
using Authorization.API.RequestModels.Authorization;
using Authorization.Application.CQRS.Authorization.Commands;
using FastEndpoints;
using MediatR;

namespace Authorization.API.Endpoints.Authorization;

public class LogoutEndpoint(IMediator mediator):Endpoint<LogoutRequest>
{
    public override void Configure()
    {
        Post("/api/authorization/logout");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LogoutRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new LogoutCommand(request.RefreshToken), cancellationToken);

        await this.SendResponse(result, static result => new { Data = result.Value });
    }
}