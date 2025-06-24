using API.Endpoints.Base;
using Application.CQRS.Authorization.Login;
using MediatR;

namespace API.Endpoints.Login;

public class LoginEndpoint(IMediator mediator, LoginErrorMapper errorMapper):BaseEndpoint<LoginRequest, LoginResponse>
{
    public override void Configure()
    {
        Post(BaseEndpointsRoute.BaseRoute + "/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new LoginCommand(request.Email, request.Password, request.Role), cancellationToken);

        await SendResponseByResult(result, static loginCommandResult => new(loginCommandResult.AccessToken, loginCommandResult.RefreshToken), errorMapper, cancellationToken);
    }
}