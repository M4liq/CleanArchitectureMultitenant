using Application.Common.Core;
using FluentValidation;
using WebApi.Common.Base;

namespace WebApi.Identity.Endpoints.ApiClient;

public class AuthenticateApiClientValidator : BaseValidator<ApiKeyAuthRequest>
{
    public AuthenticateApiClientValidator(IRequestErrorManager requestErrorManager) 
        : base(requestErrorManager)
    {
        RuleFor(x => x.ApiKey)
            .NotEmpty()
            .WithMessage(GetErrorMessage(new ApiKeyRequired()));
    }
}

public class ApiKeyRequired : IRequestError
{
    public string Code { get; init; } = nameof(ApiKeyRequired);
    public string MessagePl { get; init; } = "Klucz API jest wymagany.";
    public string MessageEn { get; init; } = "API key is required.";
}