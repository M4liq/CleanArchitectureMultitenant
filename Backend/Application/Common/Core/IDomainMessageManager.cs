using Domain.Common.Base;
using Domain.Common.Interfaces;

namespace Application.Common.Core;

public interface IDomainMessageManager
{
    string GetMessage<T>() where T : IDomainMessage, new();
    Task<string> GetMessageAsync<T>(CancellationToken ct) where T : IDomainMessage, new();
    List<string> GetMessages<T>() where T : IDomainMessage, new();
    Task<List<string>> GetMessagesAsync<T>(CancellationToken ct) where T : IDomainMessage, new();
    Task<ValidationResult> GetValidationResultAsync<T>(CancellationToken ct) where T : IDomainMessage, new();
}