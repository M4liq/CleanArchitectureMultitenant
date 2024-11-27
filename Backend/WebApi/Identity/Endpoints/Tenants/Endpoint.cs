using Application.Identity.Commands;
using Domain.Identity.ApiClient;
using MediatR;
using WebApi.Common.Base;

namespace WebApi.Identity.Endpoints.Tenants;

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
        AuthSchemes("Mixed");
        Policies(Scope.Tenants.Create.Value);
    }

    public override async Task HandleAsync(
        CreateTenantRequest req,
        CancellationToken ct)
    {
        var command = new CreateTenant.CreateTenantCommand(
            req.Name);

        var result = await _mediator.Send(command, ct);
        await SendAsync(result, cancellation: ct);
    }
}