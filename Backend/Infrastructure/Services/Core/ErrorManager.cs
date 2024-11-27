using Application.Common.Core;
using Application.Common.Settings;
using Domain.Common;
using Domain.Common.Base;
using Domain.Common.Interfaces;

namespace Infrastructure.Services.Core;

public class ErrorManager : IErrorManager
{
    private readonly IErrorsAndMessagesSettings _errorsAndMessagesSettings;

    public ErrorManager(IErrorsAndMessagesSettings errorsAndMessagesSettings)
    {
        _errorsAndMessagesSettings = errorsAndMessagesSettings;
    }

    public string GetMessageForError<T>() where T : IDomainError, new()
    {
        var messages = GetMessagesForError<T>();
        return messages.First();
    }

    public async Task<string> GetMessageForErrorAsync<T>() where T : IDomainError, new()
    {
        var messages = await GetMessagesForErrorAsync<T>();
        return messages.First();
    }

    public List<string> GetMessagesForError<T>() where T : IDomainError, new()
    {
        var error = new T();
        
        if (string.Equals("en", _errorsAndMessagesSettings.DefaultLanguageCode,
                StringComparison.OrdinalIgnoreCase))
        {
            return new List<string> {error.DefaultMessageEn};
        }
        
        return new List<string> {error.DefaultMessagePl};
    }

    public async Task<List<string>> GetMessagesForErrorAsync<T>() where T : IDomainError, new()
    {
        var error = new T();
        
        if (string.Equals("en", _errorsAndMessagesSettings.DefaultLanguageCode,
                StringComparison.OrdinalIgnoreCase))
        {
            return new List<string> {error.DefaultMessageEn};
        }
        
        return new List<string> {error.DefaultMessagePl};
    }

    public async Task<ValidationResult> GetValidationResultForErrorAsync<T>() where T : IDomainError, new()
    {
        var messages = await GetMessagesForErrorAsync<T>();
        return new ValidationResult().AddErrors(messages);
    }
}