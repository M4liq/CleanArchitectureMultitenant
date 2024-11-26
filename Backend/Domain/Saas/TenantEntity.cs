using Domain.Common;

namespace Domain.Saas;

public class TenantEntity : BaseEntity
{
    public string Name { get; set; }
    public string ConnectionString { get; set; }
    public Guid ApiKey { get; set; }
}