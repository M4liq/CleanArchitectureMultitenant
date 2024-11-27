using Domain.Common.Interfaces;

namespace Domain.Identity.RefreshToken;

public static class  ApplicationRefreshTokenDomainErrors
{
    public class JwtTokenIsNoValid : IDomainError
    {
        public string Code { get; init; } = nameof(JwtTokenIsNoValid);
        public string DefaultMessagePl { get; init; } = "Nie prawidłowy token Jwt.";
        public string DefaultMessageEn { get; init; } = "Jwt token is not valid.";
    }
    
    public class RefreshTokenDoesNotExits : IDomainError
    {
        public string Code { get; init; } = nameof(RefreshTokenDoesNotExits);
        public string DefaultMessagePl { get; init; } = "Podany token odświeżający nie istnieje.";
        public string DefaultMessageEn { get; init; } = "This refresh token does not exist.";
    }
    
    public class RefreshTokenHasExpired : IDomainError
    {
        public string Code { get; init; } = nameof(RefreshTokenHasExpired);
        public string DefaultMessagePl { get; init; } = "Podany token odświeżający jest przedawniony.";
        public string DefaultMessageEn { get; init; } = "This refresh token has expired.";
    }
    
    public class RefreshTokenHasBeenInvalidated : IDomainError
    {
        public string Code { get; init; } = nameof(RefreshTokenHasBeenInvalidated);
        public string DefaultMessagePl { get; init; } = "Podany token odświeżający został unieważniony";
        public string DefaultMessageEn { get; init; } = "This refresh token has been invalidated.";
    }
    
    public class RefreshTokenHasBeenUsed : IDomainError
    {
        public string Code { get; init; } = nameof(RefreshTokenHasBeenUsed);
        public string DefaultMessagePl { get; init; } = "Podany token odświeżający był już użyty.";
        public string DefaultMessageEn { get; init; } = "This refresh token has been used.";
    }
    
    public class RefreshTokenDoesNotMatchJwt : IDomainError
    {
        public string Code { get; init; } = nameof(RefreshTokenDoesNotMatchJwt);
        public string DefaultMessagePl { get; init; } = "Podany token odświeżający nie pasuje do połączonego z nim Jwt.";
        public string DefaultMessageEn { get; init; } = "This refresh token does not match this Jwt.";
    }
}