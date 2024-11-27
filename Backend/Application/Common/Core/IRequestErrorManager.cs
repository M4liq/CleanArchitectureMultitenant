namespace Application.Common.Core;

public interface IRequestError
{
    string Code { get; }
    string MessagePl { get; }
    string MessageEn { get; }
}

public interface IRequestErrorManager
{
    string GetErrorMessage(IRequestError error);
    List<string> GetErrorMessages(params IRequestError[] errors);
}