using Domain.Identity.Tenant;
using MediatR;
using WebApi.Common.Base;

namespace WebApi.Identity.CreateTenant;

public class CreateTenantEndpoint : BaseEndpoint<CreateTenantRequest, Application.Identity.Commands.CreateTenant.CreateTenantResponse>
{
    private readonly IMediator _mediator;

    public CreateTenantEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("/tenants/create");
        Policies(TenantScopeValueObject.Create);
    }

    protected override async Task<Application.Identity.Commands.CreateTenant.CreateTenantResponse> ExecuteAsync(
        CreateTenantRequest req, 
        CancellationToken ct)
    {
        var command = new Application.Identity.Commands.CreateTenant.CreateTenantCommand(req.Name);
        return await _mediator.Send(command, ct);
    }
}