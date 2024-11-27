using Application.Common.Core;
using Application.Common.Settings;

namespace Infrastructure.Services.Core;

public record RequestError : IRequestError
{
    public string Code { get; init; }
    public string MessagePl { get; init; }
    public string MessageEn { get; init; }

    public RequestError(string code, string messagePl, string messageEn)
    {
        Code = code;
        MessagePl = messagePl;
        MessageEn = messageEn;
    }
}

public class RequestErrorManager : IRequestErrorManager
{
    private readonly IErrorsAndMessagesSettings _errorsAndMessagesSettings;
    
    public RequestErrorManager(IErrorsAndMessagesSettings errorsAndMessagesSettings)
    {
        _errorsAndMessagesSettings = errorsAndMessagesSettings;
    }

    public string GetErrorMessage(IRequestError error)
    {
        return IsEnglish ? error.MessageEn : error.MessagePl;
    }

    public List<string> GetErrorMessages(params IRequestError[] errors)
    {
        return errors.Select(GetErrorMessage).ToList();
    }

    private bool IsEnglish => string.Equals(
        _errorsAndMessagesSettings.DefaultLanguageCode,
        "en",
        StringComparison.OrdinalIgnoreCase);
}