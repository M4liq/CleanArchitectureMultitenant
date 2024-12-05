using MediatR;
using WebApi.Common.Base;

namespace WebApi.Identity.AuthenticateApiClient;

public class AuthenticateApiClientEndpoint 
    : BaseEndpoint<ApiKeyAuthRequest, Application.Identity.Commands.AuthenticateApiClient.AuthenticateApiClientResponse>
{
    private readonly IMediator _mediator;

    public AuthenticateApiClientEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("/auth/api-key");
        AllowAnonymous();
    }
    
    protected override async Task<Application.Identity.Commands.AuthenticateApiClient.AuthenticateApiClientResponse> ExecuteAsync(
        ApiKeyAuthRequest req, 
        CancellationToken ct)
    {
        var command = new Application.Identity.Commands.AuthenticateApiClient.AuthenticateApiClientCommand(req.ApiKey);
        return await _mediator.Send(command, ct);
    }
}