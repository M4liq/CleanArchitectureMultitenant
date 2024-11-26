using Application.Common.Interfaces.Core;

namespace Domain.Identity;

public static class ApplicationUserDomainErrors 
{
    public class IncorrectEmailLoginError : IDomainError
    {
        public string Code { get; init; } = nameof(IncorrectEmailLoginError);
        public string DefaultMessagePl { get; init; } = "Nieprawidłowa kombinacja loginu i hasła.";
        public string DefaultMessageEn { get; init; } = "Incorrect login and password combination.";
    }

    public class UnconfirmedEmailError : IDomainError
    {
        public string Code { get; init; } = nameof(UnconfirmedEmailError);
        public string DefaultMessagePl { get; init; } = "Użytkownik nie potwierdził adresu e-mail.";
        public string DefaultMessageEn { get; init; } = "User did not confirm the email address.";
    }
    
    public class IncorrectUserPasswordError : IDomainError
    {
        public string Code { get; init; } = nameof(IncorrectUserPasswordError);
        public string DefaultMessagePl { get; init; } = "Nieprawidłowa kombinacja loginu i hasła.";
        public string DefaultMessageEn { get; init; } = "Incorrect login and password combination.";
    }

    public class UserWithGivenEmailCurrentlyExists : IDomainError
    {
        public string Code { get; init; } = nameof(UserWithGivenEmailCurrentlyExists);

        public string DefaultMessagePl { get; init; } =
            "Użytkownik z tym adresem e-mail już istnieje. Podaj inny adres e-mail.";

        public string DefaultMessageEn { get; init; } =
            "User with this email currently exists. Please provide other email address.";
    }
    
    public class IncorrectUserIdError : IDomainError
    {
        public string Code { get; init; } = nameof(IncorrectUserIdError);
        public string DefaultMessagePl { get; init; } = "Nie można znaleźć użytkownika o tym identyfikatorze.";
        public string DefaultMessageEn { get; init; } = "Could not find user with this id.";
    }
}