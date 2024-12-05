using System.Security.Claims;
using Application.Common;
using Application.Common.Core;
using Application.Identity.BoundedServices;
using Application.Identity.Builders;
using Domain.Common.Base;
using Domain.Identity.Tenant;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Identity.Commands;

public static class SwitchTenant
{
    public record SwitchTenantCommand(Guid TenantId) : IRequest<Response>;

    public class Validator : IValidationHandler<SwitchTenantCommand>
    {
        private readonly IDomainMessageManager _domainMessageManager;
        private readonly IDataContext _dataContext;
        

        public Validator(IDomainMessageManager domainMessageManager, IDataContext dataContext)
        {
            _domainMessageManager = domainMessageManager;
            _dataContext = dataContext;
        }

        public async Task<ValidationResult> Validate(SwitchTenantCommand request, CancellationToken ct)
        {
            var tenantExists = await _dataContext.Tenants
                .AnyAsync(tenant => 
                    tenant.Id == request.TenantId && tenant.IsActive, ct);

            if (tenantExists == false)
            {
                return await _domainMessageManager
                    .GetValidationResultAsync<TenantDomainErrors.UnauthorizedTenantSwitch>(ct);
            }

            return new ValidationResult();
        }
    }
    
    public class Handler : IRequestHandler<SwitchTenantCommand, Response>
    {
        private readonly IDataContext _dataContext;
        private readonly ICurrentIdentity _currentIdentity;
        private readonly IAuthenticationTokenService _tokenService;

        public Handler(IDataContext dataContext, ICurrentIdentity currentIdentity, IAuthenticationTokenService tokenService)
        {
            _dataContext = dataContext;
            _currentIdentity = currentIdentity;
            _tokenService = tokenService;
        }

        public async Task<Response> Handle(SwitchTenantCommand request, CancellationToken ct)
        {
            var tenant = await _dataContext.Tenants
                .FirstAsync(tenant => 
                    tenant.Id == request.TenantId && tenant.IsActive, ct);
            
            var claims = ClaimsBuilder
                .CreateClaims(_currentIdentity.IdentityName, _currentIdentity.IdentityId, tenant.Id, _currentIdentity.IdentityType);

            var tokenResult = await _tokenService.GenerateTokenAsync(
                new ClaimsIdentity(claims), ct);

            return new Response 
            { 
                NewToken = tokenResult.Token,
                NewRefreshToken = tokenResult.RefreshToken
            };
        }
    }

    public record Response : BaseResponse
    {
        public string NewToken { get; init; }
        public string NewRefreshToken { get; init; }
    }
}