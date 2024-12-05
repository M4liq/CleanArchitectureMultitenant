using Domain.Common.Interfaces;

namespace Domain.Identity.Authentication;

public class GenerateTokenDomainErrors
{
    public class NullOrEmptyClaimsMessage : IDomainMessage
    {
        public string Code { get; init; } = nameof(NullOrEmptyClaimsMessage);
        public string DefaultMessageEn { get; init; } = "Claims can not be empty.";
        public string DefaultMessagePl { get; init; } = "Przywileje nie mogą być puste.";
    }
}