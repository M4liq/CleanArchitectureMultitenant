using Application.Common.Settings;

namespace Infrastructure.Settings;

public class DataContextSettings : IDataContextSettings
{
    public string ConnectionString { get; set; }
}