using Domain.Common.Base;

namespace Application.Common.Core;

public interface IValidationHandler { }

public interface IValidationHandler<T> : IValidationHandler
{
    Task<ValidationResult> Validate(T request, CancellationToken ct);
}