namespace Application.Common.Interfaces.Settings;

public interface IJwtSettings
{
    public string Secret { get; set; }
    public TimeSpan JwtTokenLifeTime { get; set; }
    public TimeSpan RefreshTokenLifeTime { get; set; }
}