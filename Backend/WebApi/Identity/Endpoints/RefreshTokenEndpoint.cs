using Application.Common.Core;
using Application.Identity.Commands;
using FluentValidation;
using MediatR;
using WebApi.Common.Base;

namespace WebApi.Identity.Endpoints;

public class RefreshTokenRequest
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}

public class RefreshTokenIsEmpty : IRequestError
{
    public string Code { get; init; } = nameof(RefreshTokenIsEmpty);
    public string MessagePl { get; init; } = "Token odświeżający nie może być pusty.";
    public string MessageEn { get; init; } = "Refresh token can not be empty.";
}

public class TokenIsEmpty : IRequestError
{
    public string Code { get; init; } = nameof(TokenIsEmpty);
    public string MessagePl { get; init; } = "Token nie może być pusty.";
    public string MessageEn { get; init; } = "Token can not be empty.";
}

public class RefreshTokenValidator : BaseValidator<RefreshTokenRequest>
{
    public RefreshTokenValidator(IRequestErrorManager requestErrorManager) : base(requestErrorManager)
    {
        RuleFor(x => x.Token)
            .NotEmpty()
            .WithMessage(GetErrorMessage(new TokenIsEmpty()));

        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage(GetErrorMessage(new RefreshTokenIsEmpty()));
    }
}

public class RefreshTokenEndpoint : BaseEndpoint<RefreshTokenRequest, RefreshToken.RefreshTokenResponse>
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
            .ProducesValidationProblem()
            .Produces<RefreshToken.RefreshTokenResponse>(200, "application/json")
            .Produces<RefreshToken.RefreshTokenResponse>(400, "application/json"));
    }

    public override async Task HandleAsync(RefreshTokenRequest req, CancellationToken ct)
    {
        var command = new RefreshToken.RefreshTokenCommand(req.Token, req.RefreshToken);
        var result = await _mediator.Send(command, ct);

        await SendAsync(new RefreshToken.RefreshTokenResponse
        {
            Messages = result.Messages,
            StatusCode = result.StatusCode,
            Token = result.Token,
            RefreshToken = result.RefreshToken
        }, cancellation: ct);
    }
}