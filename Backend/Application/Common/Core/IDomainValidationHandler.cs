using Domain.Common.Base;

namespace Application.Common.Core;

public interface IDomainValidationHandler { }

public interface IDomainValidationHandler<T> : IDomainValidationHandler
{
    Task<ValidationResult> Validate(T request);
}