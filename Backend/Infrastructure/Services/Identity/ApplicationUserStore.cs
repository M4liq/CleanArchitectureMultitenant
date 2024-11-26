using Domain.Identity;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Infrastructure.Services.Identity;

public class ApplicationUserStore : UserStore<ApplicationUserEntity, ApplicationRoleEntity, DataContext, Guid,
    ApplicationUserClaimsEntity, ApplicationUserRolesEntity, ApplicationUserLoginsEntity, ApplicationUserTokensEntity, ApplicationRoleClaimsEntity>
{
    public ApplicationUserStore(DataContext context) : base(context)
    {
    }
}