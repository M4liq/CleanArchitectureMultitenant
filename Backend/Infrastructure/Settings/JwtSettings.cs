using Application.Common.Settings;

namespace Infrastructure.Settings;

public class JwtSettings : IJwtSettings
{
    public string Secret { get; set; }
    public TimeSpan JwtTokenLifeTime { get; set; }
    public TimeSpan RefreshTokenLifeTime { get; set; }
}