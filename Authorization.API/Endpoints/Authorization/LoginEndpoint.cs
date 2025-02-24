using Authorization.API.Extensions;
using Authorization.API.RequestModels.Authorization;
using Authorization.API.ResponseModels.Authorization;
using Authorization.Application.CQRS.Authorization.Commands;
using FastEndpoints;
using MediatR;

namespace Authorization.API.Endpoints.Authorization;

public class LoginEndpoint(IMediator mediator):Endpoint<LoginRequest, LoginResponse>
{
    public override void Configure()
    {
        Post("/api/authorization/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new LoginCommand(request.Login, request.Password), cancellationToken);

        await this.SendResponse(result, static result => new LoginResponse(result.Value.AccessToken, result.Value.RefreshToken));
    }
}