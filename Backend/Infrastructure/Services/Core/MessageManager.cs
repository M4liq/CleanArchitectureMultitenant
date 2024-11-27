using Application.Common.Core;
using Application.Common.Settings;
using Domain.Common;
using Domain.Common.Base;
using Domain.Common.Interfaces;

namespace Infrastructure.Services.Core;

public class MessageManager : IMessageManager
{
    private readonly IErrorsAndMessagesSettings _errorsAndMessagesSettings;

    public MessageManager(IErrorsAndMessagesSettings errorsAndMessagesSettings)
    {
        _errorsAndMessagesSettings = errorsAndMessagesSettings;
    }

    public async Task<List<string>> GetMessagesForAsync<T>() where T : IDomainMessage, new()
    {
        var error = new T();

        if (string.Equals("en", _errorsAndMessagesSettings.DefaultLanguageCode,
                StringComparison.OrdinalIgnoreCase))
        {
            return new List<string> {error.DefaultMessageEn};
        }
            
        return new List<string> {error.DefaultMessagePl};
    }

    public async Task<K> GetMessagesForAsync<T, K>() where T : IDomainMessage, new() where K : BaseResponse, new()
    {
        var messages = await GetMessagesForAsync<T>();
        var response = new K();
        response.Messages.AddRange(messages);
        
        return response;
    }
}