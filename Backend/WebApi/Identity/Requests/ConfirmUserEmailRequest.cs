using Newtonsoft.Json;

namespace WebApi.Identity.Requests;

public class ConfirmUserEmailRequest
{
    [JsonProperty("token")]
    public string Token { get; set; }

    [JsonProperty("userId")]
    public Guid UserId { get; set; }
}