using Domain.Common;

namespace Application.Common.Interfaces.Core;

public interface IErrorManager
{
    Task<List<string>> GetMessagesForErrorAsync<T>() where T : IDomainError, new();
    Task<ValidationResult> GetValidationResultForErrorAsync<T>() where T : IDomainError, new();
}