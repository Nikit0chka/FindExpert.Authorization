using Authorization.API.Extensions;
using Authorization.API.RequestModels.Authorization;
using Authorization.API.ResponseModels.Authorization;
using Authorization.Application.CQRS.Authorization.Commands;
using FastEndpoints;
using MediatR;

namespace Authorization.API.Endpoints.Authorization;

public class RefreshEndpoint(IMediator mediator):Endpoint<RefreshRequest, RefreshResponse>
{
    public override void Configure()
    {
        Post("/api/authorization/refresh");
        AllowAnonymous();
    }

    public override async Task HandleAsync(RefreshRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new RefreshCommand(request.RefreshToken), cancellationToken);

        await this.SendResponse(result, static result => new RefreshResponse(result.Value));
    }
}