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

    public class Validator : IDomainValidationHandler<CreateTenantCommand>
    {
        private readonly IErrorManager _errorManager;
        private readonly IDataContext _dataContext;

        public Validator(IErrorManager errorManager, IDataContext dataContext)
        {
            _errorManager = errorManager;
            _dataContext = dataContext;
        }

        public async Task<ValidationResult> Validate(CreateTenantCommand request)
        {
            if (await _dataContext.Tenants.AnyAsync(_ => _.Name == request.Name))
            {
                return await _errorManager
                    .GetValidationResultForErrorAsync<TenantDomainErrors.TenantWithSuchNameAlreadyExists>();
            }

            return new ValidationResult();
        }
    }

    public class Handler : IRequestHandler<CreateTenantCommand, CreateTenantResponse>
    {
        private readonly IDataContext _dataContext;

        public Handler(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<CreateTenantResponse> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
        {
            var tenant = new TenantEntity
            {
                Name = request.Name,
                IsActive = true
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