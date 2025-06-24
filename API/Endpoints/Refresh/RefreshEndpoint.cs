using API.Endpoints.Base;
using Application.CQRS.Authorization.Refresh;
using MediatR;

namespace API.Endpoints.Refresh;

public class RefreshEndpoint(IMediator mediator, RefreshErrorMapper errorMapper):BaseEndpoint<RefreshRequest, RefreshResponse>
{
    public override void Configure()
    {
        Post(BaseEndpointsRoute.BaseRoute + "/refresh");
        AllowAnonymous();
    }

    public override async Task HandleAsync(RefreshRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new RefreshCommand(request.RefreshToken), cancellationToken);

        await SendResponseByResult(result, static refreshCommandResult => new(refreshCommandResult.AccessToken), errorMapper, cancellationToken);
    }
}