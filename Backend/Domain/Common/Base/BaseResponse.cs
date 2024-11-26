using System.Net;

namespace Domain.Common;

public record BaseResponse
{
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
    public List<string> Messages { get; init; } = new List<string>();

    public void Fail(List<string> errors)
    {
        if (errors.Any())
        {
            StatusCode = HttpStatusCode.BadRequest;
        }
        
        Messages.AddRange(errors);
    }
}