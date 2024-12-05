using Application.Common.Settings;

namespace Infrastructure.Services.Core;

public interface ICurrentLanguage
{
    string GetCurrentLanguage();
    void SetCurrentLanguage(string language);
}

public class CurrentLanguage : ICurrentLanguage
{
    private static readonly AsyncLocal<string> _currentLanguage = new();
    private readonly IErrorsAndMessagesSettings _settings;

    public CurrentLanguage(IErrorsAndMessagesSettings settings)
    {
        _settings = settings;
    }

    public string GetCurrentLanguage()
    {
        return _currentLanguage.Value ?? _settings.DefaultLanguageCode;
    }

    public void SetCurrentLanguage(string language)
    {
        _currentLanguage.Value = language?.ToLower();
    }
}