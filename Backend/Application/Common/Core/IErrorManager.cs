using Domain.Common.Base;
using Domain.Common.Interfaces;

namespace Application.Common.Core;

public interface IErrorManager
{
    string GetMessageForError<T>() where T : IDomainError, new();
    Task<string> GetMessageForErrorAsync<T>() where T : IDomainError, new();
    List<string> GetMessagesForError<T>() where T : IDomainError, new();
    Task<List<string>> GetMessagesForErrorAsync<T>() where T : IDomainError, new();
    Task<ValidationResult> GetValidationResultForErrorAsync<T>() where T : IDomainError, new();
}