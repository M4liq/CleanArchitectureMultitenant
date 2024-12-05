using MediatR;

namespace WebApi.Identity.Endpoints.ApiClient;

public class AuthenticateApiClientEndpoint 
    : BaseEndpoint<ApiKeyAuthRequest, AuthenticateApiClient.AuthenticateApiClientResponse>
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
    
    protected override async Task<AuthenticateApiClient.AuthenticateApiClientResponse> ExecuteAsync(
        ApiKeyAuthRequest req, 
        CancellationToken ct)
    {
        var command = new AuthenticateApiClient.AuthenticateApiClientCommand(req.ApiKey);
        return await _mediator.Send(command, ct);
    }
}