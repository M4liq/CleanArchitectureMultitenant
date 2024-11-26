using Application.Common.Interfaces.Settings;

namespace Infrastructure.Settings;

public class SaasSettings : ISaasSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
}