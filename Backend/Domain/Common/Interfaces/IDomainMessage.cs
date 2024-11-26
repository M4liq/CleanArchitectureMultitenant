namespace Application.Common.Interfaces.Core;

public interface IDomainMessage
{
    public string Code { get; init; }
    public string DefaultMessagePl { get; init; }
    public string DefaultMessageEn { get; init; }
}