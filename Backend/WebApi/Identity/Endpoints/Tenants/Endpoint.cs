using Application.Identity.Commands;
using Domain.Identity.ApiClient;
using MediatR;
using WebApi.Identity.Endpoints.Tenants;

public class CreateTenantEndpoint : BaseEndpoint<CreateTenantRequest, CreateTenant.CreateTenantResponse>
{
    private readonly IMediator _mediator;

    public CreateTenantEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("/tenants");
        Policies(Scope.Tenants.Create.Value);
    }

    protected override async Task<CreateTenant.CreateTenantResponse> ExecuteAsync(
        CreateTenantRequest req, 
        CancellationToken ct)
    {
        var command = new CreateTenant.CreateTenantCommand(req.Name);
        return await _mediator.Send(command, ct);
    }
}