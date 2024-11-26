using Application.Common.Interfaces.Core;

namespace Domain.Identity;

public class ApplicationUserDomainMessages
{
    public class MailChangePasswordTokenDomainMessage : IDomainMessage
    {
        public string Code { get; init; } = nameof(MailChangePasswordTokenDomainMessage);
        public string DefaultMessagePl { get; init; } = "Jeśli użytkownik istnieje, to na podany adress email została wysłana wiadomość z dalszymi instrukcjami.";
        public string DefaultMessageEn { get; init; } = "If user exists, email containing futher instructions was sent.";
    }
    
    public class MailEmailConfirmationDomainMessage : IDomainMessage
    {
        public string Code { get; init; } = nameof(MailEmailConfirmationDomainMessage);
        public string DefaultMessagePl { get; init; } = "Jeśli użytkownik istnieje, to na podany adress email została wysłana wiadomość z dalszymi instrukcjami.";
        public string DefaultMessageEn { get; init; } = "If user exists, email containing futher instructions was sent.";
    }
}