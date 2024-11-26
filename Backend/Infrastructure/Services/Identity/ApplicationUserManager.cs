using Application.Common.Dtos;
using Application.Common.Interfaces.Services;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services.Identity;

public class ApplicationUserManager : UserManager<ApplicationUserEntity>, IApplicationUserManager
{
    public ApplicationUserManager(IUserStore<ApplicationUserEntity> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<ApplicationUserEntity> passwordHasher,
        IEnumerable<IUserValidator<ApplicationUserEntity>> userValidators, IEnumerable<IPasswordValidator<ApplicationUserEntity>> passwordValidators,
        ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<ApplicationUserEntity>> logger)

        : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    {
    }

    public async Task<ApplicationUserEntity> FindByIdAsync(Guid userId)
    {
        return await base.FindByIdAsync(userId.ToString());
    }

    public async Task<IdentityActionResultDto> CreateAsync(ApplicationUserEntity user, string requestPassword)
    {
        var result = await base.CreateAsync(user, requestPassword);

        return new IdentityActionResultDto
        {
            Success = result.Succeeded,
            Errors = result.Errors.Select(e => e.Description).ToList()
        };
    }

    public async Task<IdentityActionResultDto> ResetPasswordAsync(ApplicationUserEntity user, string token, string newPassword)
    {
        var result = await base.ResetPasswordAsync(user, token, newPassword);
        
        return new IdentityActionResultDto
        {
            Success = result.Succeeded,
            Errors = result.Errors.Select(e => e.Description).ToList()
        };
    }
    
}