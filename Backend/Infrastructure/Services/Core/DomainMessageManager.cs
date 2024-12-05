using Application.Common.Core;
using Domain.Common.Base;
using Domain.Common.Interfaces;

namespace Infrastructure.Services.Core;

public class DomainMessageManager : IDomainMessageManager
{
    private readonly ICurrentLanguage _currentLanguage;

    public DomainMessageManager(ICurrentLanguage currentLanguage)
    {
        _currentLanguage = currentLanguage;
    }

    public string GetMessage<T>() where T : IDomainMessage, new()
    {
        var messages = GetMessages<T>();
        return messages.First();
    }

    public async Task<string> GetMessageAsync<T>(CancellationToken ct) where T : IDomainMessage, new()
    {
        var messages = await GetMessagesAsync<T>(ct);
        return messages.First();
    }

    public List<string> GetMessages<T>() where T : IDomainMessage, new()
    {
        var message = new T();
        
        return _currentLanguage.GetCurrentLanguage() switch
        {
            "en" => new List<string> { message.DefaultMessageEn },
            "pl" => new List<string> { message.DefaultMessagePl },
            _ => new List<string> { message.DefaultMessageEn }
        };
    }

    public async Task<List<string>> GetMessagesAsync<T>(CancellationToken ct) where T : IDomainMessage, new()
    {
        var message = new T();
        
        return _currentLanguage.GetCurrentLanguage() switch
        {
            "en" => new List<string> { message.DefaultMessageEn },
            "pl" => new List<string> { message.DefaultMessagePl },
            _ => new List<string> { message.DefaultMessageEn }
        };
    }

    public async Task<ValidationResult> GetValidationResultAsync<T>(CancellationToken ct) where T : IDomainMessage, new()
    {
        var messages = await GetMessagesAsync<T>(ct);
        return new ValidationResult().AddErrors(messages);
    }
}