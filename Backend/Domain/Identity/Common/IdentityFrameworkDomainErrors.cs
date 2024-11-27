using Domain.Common.Interfaces;

namespace Domain.Identity.Common;

public static class IdentityFrameworkDomainErrors
{
    public class DefaultError : IDomainError
    {
        public string Code { get; init; } = nameof(DefaultError);
        public string DefaultMessagePl { get; init; } = "Wystąpił nieanany błąd.";
        public string DefaultMessageEn { get; init; } = "An unknown error occurred.";
    }
    public class ConcurrencyFailure : IDomainError
    {
        public string Code { get; init; } = nameof(ConcurrencyFailure);
        public string DefaultMessagePl { get; init; } = "Błąd współbieżności, obiekt został zmodyfikowany.";
        public string DefaultMessageEn { get; init; } = "Concurrency error, object has been modified.";
    }
    public class PasswordMismatch : IDomainError
    {
        public string Code { get; init; } = nameof(PasswordMismatch);
        public string DefaultMessagePl { get; init; } = "Nieprawidłowe hasło.";
        public string DefaultMessageEn { get; init; } = "Incorrect password.";
    }
    
    public class DuplicateEmail : IDomainError
    {
        public string Code { get; init; } = nameof(DuplicateEmail);
        public string DefaultMessagePl { get; init; } = "Adres {0} jest zajęty.";
        public string DefaultMessageEn { get; init; } = "The address {0} is taken.";
    }
    
    public class PasswordRequiresUpper : IDomainError
    {
        public string Code { get; init; } = nameof(PasswordRequiresUpper);
        public string DefaultMessagePl { get; init; } = "Hasło musi posiadać przynajmniej jedną wielką literę ('A'-'Z').";
        public string DefaultMessageEn { get; init; } = "The password must contain at least one capital letter ('A'-'Z').";
    }
    
    public class PasswordTooShort : IDomainError
    {
        public string Code { get; init; } = nameof(PasswordTooShort);
        public string DefaultMessagePl { get; init; } = "Hasło musi posiadać conajmniej {0} znaków.";
        public string DefaultMessageEn { get; init; } = "The password must contain at least {0} characters.";
    }
    
    public class InvalidToken : IDomainError
    {
        public string Code { get; init; } = nameof(InvalidToken);
        public string DefaultMessagePl { get; init; } = "Nieprawidłowy token.";
        public string DefaultMessageEn { get; init; } = "Invalid token.";
    }
    
    public class LoginAlreadyAssociated : IDomainError
    {
        public string Code { get; init; } = nameof(LoginAlreadyAssociated);
        public string DefaultMessagePl { get; init; } = "Użytkownik o takiej nazwie już istnieje.";
        public string DefaultMessageEn { get; init; } = "A user with that name already exists.";
    }
    
    public class InvalidUserName : IDomainError
    {
        public string Code { get; init; } = nameof(InvalidUserName);
        public string DefaultMessagePl { get; init; } = "Nazwa użytkownika {0} jest nieprawidłowa, może posiadać tylko znaki i cyfry.";
        public string DefaultMessageEn { get; init; } = "The username {0} is invalid, it can only contain characters and numbers.";
    }
    
    public class InvalidEmail : IDomainError
    {
        public string Code { get; init; } = nameof(InvalidEmail);
        public string DefaultMessagePl { get; init; } = "Email {0} jest nieprawidłowy.";
        public string DefaultMessageEn { get; init; } = "Email {0} is invalid.";
    }
    
    public class DuplicateUserName : IDomainError
    {
        public string Code { get; init; } = nameof(DuplicateUserName);
        public string DefaultMessagePl { get; init; } = "Nazwa użytkownika {0} jest zajęta.";
        public string DefaultMessageEn { get; init; } = "Username {0} is taken.";
    }
    
    public class InvalidRoleName : IDomainError
    {
        public string Code { get; init; } = nameof(InvalidRoleName);
        public string DefaultMessagePl { get; init; } = "Nieprawidłowa nazwa roli: {0}.";
        public string DefaultMessageEn { get; init; } = "Role name is invalid: {0}.";
    }
    
    public class DuplicateRoleName : IDomainError
    {
        public string Code { get; init; } = nameof(DuplicateRoleName);
        public string DefaultMessagePl { get; init; } = "Nazwa grupy {0} jest zajęta.";
        public string DefaultMessageEn { get; init; } = "Group name {0} is taken.";
    }
    
    public class UserAlreadyHasPassword : IDomainError
    {
        public string Code { get; init; } = nameof(UserAlreadyHasPassword);
        public string DefaultMessagePl { get; init; } = "Hasło użytkownika jest już ustawione.";
        public string DefaultMessageEn { get; init; } = "User password is already set.";
    }
    
    public class UserLockoutNotEnabled : IDomainError
    {
        public string Code { get; init; } = nameof(UserLockoutNotEnabled);
        public string DefaultMessagePl { get; init; } = "Blokada nie jest ustawiona dla tego użytkownika.";
        public string DefaultMessageEn { get; init; } = "A lock is not set for this user.";
    }
    
    public class UserAlreadyInRole : IDomainError
    {
        public string Code { get; init; } = nameof(UserAlreadyInRole);
        public string DefaultMessagePl { get; init; } = "Użytkownik ma już przypisaną grupę {0}.";
        public string DefaultMessageEn { get; init; } = "The user already has group {0} assigned.";
    }
    
    public class UserNotInRole : IDomainError
    {
        public string Code { get; init; } = nameof(UserNotInRole);
        public string DefaultMessagePl { get; init; } = "Użytkownik nie należy do grupy {0}.";
        public string DefaultMessageEn { get; init; } = "The user does not belong to the group {0}.";
    }
    
    public class PasswordRequiresNonAlphanumeric : IDomainError
    {
        public string Code { get; init; } = nameof(PasswordRequiresNonAlphanumeric);
        public string DefaultMessagePl { get; init; } = "Hasło musi posiadać przynajmniej jeden znak alfanumeryczny.";
        public string DefaultMessageEn { get; init; } = "The password must contain at least one alphanumeric character.";
    }
    
    public class PasswordRequiresDigit : IDomainError
    {
        public string Code { get; init; } = nameof(PasswordRequiresDigit);
        public string DefaultMessagePl { get; init; } = "Hasło musi posiadać przynajmniej jedną cyfrę ('0'-'9').";
        public string DefaultMessageEn { get; init; } = "The password must contain at least one number ('0'-'9').";
    }
    
    public class PasswordRequiresLower : IDomainError
    {
        public string Code { get; init; } = nameof(PasswordRequiresLower);
        public string DefaultMessagePl { get; init; } = "Hasło musi posiadać przynajmniej jedną małą literę ('a'-'z').";
        public string DefaultMessageEn { get; init; } = "The password must contain at least one lowercase letter ('a'-'z').";
    }
    
    public class PasswordRequiresUniqueChars : IDomainError
    {
        public string Code { get; init; } = nameof(PasswordRequiresUniqueChars);
        public string DefaultMessagePl { get; init; } = "Hasło musi posiadać przynajmniej jeden znak specjalny.";
        public string DefaultMessageEn { get; init; } = "The password must contain at least one special character.";
    }
}