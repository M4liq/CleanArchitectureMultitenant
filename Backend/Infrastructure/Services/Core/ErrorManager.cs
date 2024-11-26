using Application.Common.Interfaces.Core;
using Application.Common.Interfaces.Settings;
using Domain.Common;

namespace Infrastructure.Services.Core;

public class ErrorManager : IErrorManager
{
    private readonly IErrorsAndMessagesSettings _errorsAndMessagesSettings;

    public ErrorManager(IErrorsAndMessagesSettings errorsAndMessagesSettings)
    {
        _errorsAndMessagesSettings = errorsAndMessagesSettings;
    }

    public async Task<List<string>> GetMessagesForErrorAsync<T>() where T : IDomainError, new()
    {
        var error = new T();
        
        if (string.Equals("en", _errorsAndMessagesSettings.DefaultErrorLanguageCode,
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