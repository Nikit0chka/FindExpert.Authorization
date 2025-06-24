using API.Endpoints.Base;
using Application.CQRS.Authorization.Logout;
using MediatR;

namespace API.Endpoints.Logout;

public class LogoutEndpoint(IMediator mediator, LogoutErrorMapper errorMapper):BaseEndpoint<LogoutRequest>
{
    public override void Configure()
    {
        Post(BaseEndpointsRoute.BaseRoute + "/logout");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LogoutRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new LogoutCommand(request.RefreshToken), cancellationToken);

        await SendResponseByResult(result, errorMapper, cancellationToken);
    }
}