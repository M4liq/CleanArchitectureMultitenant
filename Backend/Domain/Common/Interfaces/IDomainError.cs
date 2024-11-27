namespace Domain.Common.Interfaces;

public interface IDomainError
{
    public string Code { get; init; }
    public string DefaultMessagePl { get; init; }
    public string DefaultMessageEn { get; init; }
}