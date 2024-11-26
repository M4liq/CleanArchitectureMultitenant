using Domain.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Common;

public interface IDataContext
{
    DbSet<ApplicationRefreshTokensEntity> RefreshTokens { get; set; }
    Task<bool> SaveChangesAsync();
}