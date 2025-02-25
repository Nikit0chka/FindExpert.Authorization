using API.Extensions;
using API.RequestModels.Authorization;
using Application.CQRS.Authorization.Commands;
using FastEndpoints;
using MediatR;

namespace API.Endpoints.Authorization;

public class RegisterEndpoint(IMediator mediator):Endpoint<RegisterRequest>
{
    public override void Configure()
    {
        Post("/api/authorization/register");
        AllowAnonymous();
    }

    public override async Task HandleAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new RegisterCommand(request.Email, request.Password), cancellationToken);

        await this.SendResponse(result, static result => new { Data = result.Value });
    }
}