using Application.Common.Settings;

namespace Infrastructure.Settings;

public class ErrorsAndMessagesSettings : IErrorsAndMessagesSettings
{
    public string DefaultLanguageCode { get; set; }
}