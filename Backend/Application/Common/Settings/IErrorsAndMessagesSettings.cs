namespace Application.Common.Interfaces.Settings;

public interface IErrorsAndMessagesSettings
{
    public string DefaultErrorLanguageCode { get; set; }
    public string DefaultMessageLanguageCode { get; set; }
}