using Application.Identity.Commands;
using Domain.Identity.Tenant;
using MediatR;
using WebApi.Common.Base;

namespace WebApi.Identity.SwttchTenant;

public class SwitchTenantEndpoint : BaseEndpoint<SwitchTenantRequest, SwitchTenant.Response>
{
    private readonly IMediator _mediator;

    public SwitchTenantEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("/tenants/switch");
        Policies(TenantScopeValueObject.Switch);
    }

    protected override async Task<SwitchTenant.Response> ExecuteAsync(
        SwitchTenantRequest req, 
        CancellationToken ct)
    {
        return await _mediator.Send(new SwitchTenant.SwitchTenantCommand(req.TenantId), ct);
    }
}