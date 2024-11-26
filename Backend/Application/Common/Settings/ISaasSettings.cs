namespace Application.Common.Interfaces.Settings;

public interface ISaasSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
}