using Application.Common.Core;
using FluentValidation;
using WebApi.Common.Base;

namespace WebApi.Identity.Endpoints.Tenants;

public class CreateTenantValidator : BaseValidator<CreateTenantRequest>
{
    public CreateTenantValidator(IRequestErrorManager requestErrorManager) 
        : base(requestErrorManager)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(GetErrorMessage(new TenantNameRequired()))
            .MaximumLength(100)
            .WithMessage(GetErrorMessage(new TenantNameTooLong()));
    }
}

public class TenantNameRequired : IRequestError
{
    public string Code { get; init; } = nameof(TenantNameRequired);
    public string MessagePl { get; init; } = "Nazwa tenanta nie może być pusta.";
    public string MessageEn { get; init; } = "Tenant name cannot be empty.";
}

public class TenantNameTooLong : IRequestError
{
    public string Code { get; init; } = nameof(TenantNameTooLong);
    public string MessagePl { get; init; } = "Nazwa tenanta nie może być dłuższa niż 100 znaków.";
    public string MessageEn { get; init; } = "Tenant name cannot be longer than 100 characters.";
}
