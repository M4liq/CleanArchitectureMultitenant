using Application.Common.Dtos;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace Application.Common.Interfaces.Services;

public interface IApplicationUserManager
{
    Task<ApplicationUserEntity> FindByIdAsync(Guid userId);
    Task<ApplicationUserEntity> FindByEmailAsync(string email);
    Task<IdentityActionResultDto> CreateAsync(ApplicationUserEntity user, string password);
    Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUserEntity user);
    Task<bool> IsEmailConfirmedAsync(ApplicationUserEntity user);
    Task<bool> CheckPasswordAsync(ApplicationUserEntity user, string password);
    Task<IdentityActionResultDto> ResetPasswordAsync(ApplicationUserEntity user, string token, string newPassword);
    Task<IdentityResult> ConfirmEmailAsync(ApplicationUserEntity user, string token);
    Task<string> GeneratePasswordResetTokenAsync(ApplicationUserEntity user);
}