using Application.Common.Core;

namespace Infrastructure.Persistence;

public static class SystemContextConfiguration
{
    public static readonly Guid SystemUserId = new("11111111-1111-1111-1111-111111111111");
    public static readonly Guid SystemTenantId = new("22222222-2222-2222-2222-222222222222");
}

// DataContext.Multitenancy.SystemContext
public partial class DataContext
{
    private bool _bypassTenantFilter;
    private SystemContext _systemContext;
    private readonly ITenantProvider _tenantProvider;

    public IDisposable UseSystemContext()
    {
        var previousBypass = _bypassTenantFilter;
        _bypassTenantFilter = true;
        _systemContext = new SystemContext(
            SystemContextConfiguration.SystemTenantId, 
            SystemContextConfiguration.SystemUserId);
        return new SystemContextScope(this, previousBypass);
    }
    
    internal void ResetSystemContext(bool previousBypassValue)
    {
        _bypassTenantFilter = previousBypassValue;
        _systemContext = null;
    }
}

// Separate class to handle the IDisposable pattern
public class SystemContextScope : IDisposable
{
    private readonly DataContext _context;
    private readonly bool _previousBypassValue;
    private bool _disposed;

    public SystemContextScope(DataContext context, bool previousBypassValue)
    {
        _context = context;
        _previousBypassValue = previousBypassValue;
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _context.ResetSystemContext(_previousBypassValue);
            _disposed = true;
        }
    }
}