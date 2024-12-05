using Domain.Common.Interfaces;

namespace Domain.Identity.Authentication;

public static class AuthenticationDomainErrors
{
    public class GeneralValidationMessage : IDomainMessage
    {
        public string Code { get; init; } = nameof(GeneralValidationMessage);
        public string DefaultMessagePl { get; init; } = "Nieudane uwierzetylnienie.";
        public string DefaultMessageEn { get; init; } = "Authentication failed.";
    }
    
    public class UserDoesNotExistValidationMessage : IDomainMessage
    {
        public string Code { get; init; } = nameof(ApiClientDoesNotExistValidationMessage);
        public string DefaultMessagePl { get; init; } = "Nieudane uwierzetylnienie.";
        public string DefaultMessageEn { get; init; } = "Authentication failed.";
    }
    
    public class ApiClientDoesNotExistValidationMessage : IDomainMessage
    {
        public string Code { get; init; } = nameof(ApiClientDoesNotExistValidationMessage);
        public string DefaultMessagePl { get; init; } = "Nieudane uwierzetylnienie.";
        public string DefaultMessageEn { get; init; } = "Authentication failed.";
    }
}
