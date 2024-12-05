using Domain.Common.Interfaces;

namespace Domain.Identity.RefreshToken;

public static class  RefreshTokenDomainErrors
{
    public class JwtTokenIsNoValid : IDomainMessage
    {
        public string Code { get; init; } = nameof(JwtTokenIsNoValid);
        public string DefaultMessagePl { get; init; } = "Nie prawidłowy token Jwt.";
        public string DefaultMessageEn { get; init; } = "Jwt token is not valid.";
    }
    
    public class RefreshTokenDoesNotExits : IDomainMessage
    {
        public string Code { get; init; } = nameof(RefreshTokenDoesNotExits);
        public string DefaultMessagePl { get; init; } = "Podany token odświeżający nie istnieje.";
        public string DefaultMessageEn { get; init; } = "This refresh token does not exist.";
    }
    
    public class RefreshTokenHasExpired : IDomainMessage
    {
        public string Code { get; init; } = nameof(RefreshTokenHasExpired);
        public string DefaultMessagePl { get; init; } = "Podany token odświeżający jest przedawniony.";
        public string DefaultMessageEn { get; init; } = "This refresh token has expired.";
    }
    
    public class RefreshTokenHasBeenInvalidated : IDomainMessage
    {
        public string Code { get; init; } = nameof(RefreshTokenHasBeenInvalidated);
        public string DefaultMessagePl { get; init; } = "Podany token odświeżający został unieważniony";
        public string DefaultMessageEn { get; init; } = "This refresh token has been invalidated.";
    }
    
    public class RefreshTokenHasBeenUsed : IDomainMessage
    {
        public string Code { get; init; } = nameof(RefreshTokenHasBeenUsed);
        public string DefaultMessagePl { get; init; } = "Podany token odświeżający był już użyty.";
        public string DefaultMessageEn { get; init; } = "This refresh token has been used.";
    }
    
    public class RefreshTokenDoesNotMatchJwt : IDomainMessage
    {
        public string Code { get; init; } = nameof(RefreshTokenDoesNotMatchJwt);
        public string DefaultMessagePl { get; init; } = "Podany token odświeżający nie pasuje do połączonego z nim Jwt.";
        public string DefaultMessageEn { get; init; } = "This refresh token does not match this Jwt.";
    }
    
    public class CouldNotGetIdentityId : IDomainMessage
    {
        public string Code { get; init; } = nameof(CouldNotGetIdentityId);
        public string DefaultMessagePl { get; init; } = "Nieprawidłowe id toższamości.";
        public string DefaultMessageEn { get; init; } = "Invalid identity id.";
    }
    
    public class CouldNotGetIdentityType : IDomainMessage
    {
        public string Code { get; init; } = nameof(CouldNotGetIdentityType);
        public string DefaultMessagePl { get; init; } = "Nieprawidłowy typ tożsamości.";
        public string DefaultMessageEn { get; init; } = "Could not get identity type.";
    }
}