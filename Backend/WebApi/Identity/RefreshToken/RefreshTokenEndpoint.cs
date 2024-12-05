using Application.Identity.Commands;
using MediatR;
using WebApi.Common.Base;

namespace WebApi.Identity.RefreshToken;

public class RefreshTokenEndpoint : BaseEndpoint<RefreshTokenRequest, RefreshTokenHandler.RefreshTokenResponse>
{
    private readonly IMediator _mediator;

    public RefreshTokenEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("/identity/refresh-token");
        AllowAnonymous();
        Description(d => d
            .WithName("RefreshToken")
            .WithTags("Identity")
            .WithDescription("Refreshes expired JWT token")
            .Produces<RefreshTokenHandler.RefreshTokenResponse>(200, "application/json")
            .Produces<RefreshTokenHandler.RefreshTokenResponse>(400, "application/json"));
    }
    
    protected override async Task<RefreshTokenHandler.RefreshTokenResponse> ExecuteAsync(
        RefreshTokenRequest req, 
        CancellationToken ct)
    {
        var command = new RefreshTokenHandler.RefreshTokenCommand(req.Token, req.RefreshToken);
        return await _mediator.Send(command, ct);
    }
}