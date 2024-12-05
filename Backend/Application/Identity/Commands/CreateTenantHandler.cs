using Application.Common;
using Application.Common.Core;
using Domain.Common.Base;
using Domain.Identity.Tenant;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Identity.Commands;

public static class CreateTenant
{
    public record CreateTenantCommand(string Name) : IRequest<CreateTenantResponse>;

    public class Validator : IValidationHandler<CreateTenantCommand>
    {
        private readonly IDomainMessageManager _domainMessageManager;
        private readonly IDataContext _dataContext;
        

        public Validator(IDomainMessageManager domainMessageManager, IDataContext dataContext)
        {
            _domainMessageManager = domainMessageManager;
            _dataContext = dataContext;
        }

        public async Task<ValidationResult> Validate(CreateTenantCommand request, CancellationToken ct)
        {
            if (await _dataContext.Tenants.AnyAsync(tenant => tenant.Name == request.Name, cancellationToken: ct))
            {
                return await _domainMessageManager
                    .GetValidationResultAsync<TenantDomainErrors.TenantWithSuchNameAlreadyExists>(ct);
            }

            return new ValidationResult();
        }
    }

    public class Handler : IRequestHandler<CreateTenantCommand, CreateTenantResponse>
    {
        private readonly IDataContext _dataContext;
        private readonly ICalendar _calendar;

        public Handler(IDataContext dataContext, ICalendar calendar)
        {
            _dataContext = dataContext;
            _calendar = calendar;
        }

        public async Task<CreateTenantResponse> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
        {
            var tenant = new TenantEntity
            {
                Name = request.Name,
                IsActive = true,
                CreatedAt = _calendar.UtcNow
            };

            _dataContext.Tenants.Add(tenant);
            await _dataContext.SaveChangesAsync(cancellationToken);
            
            return new CreateTenantResponse
            {
                TenantId = tenant.Id,
                Name = tenant.Name
            };
        }
    }

    public record CreateTenantResponse : BaseResponse
    {
        public Guid TenantId { get; init; }
        public string Name { get; init; }
    }
}