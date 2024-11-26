using Application.Common.Interfaces.Settings;

namespace Infrastructure.Settings;

public class ErrorsAndMessagesSettings : IErrorsAndMessagesSettings
{
    public string DefaultErrorLanguageCode { get; set; }
    public string DefaultMessageLanguageCode { get; set; }
}