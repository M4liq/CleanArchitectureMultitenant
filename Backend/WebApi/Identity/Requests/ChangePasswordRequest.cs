using Newtonsoft.Json;

namespace WebApi.Identity.Requests;

public class ChangePasswordRequest
{
    [JsonProperty("newPassword")]
    public string NewPassword { get; set; }

    [JsonProperty("token")]
    public string Token { get; set; }

    [JsonProperty("userId")]
    public Guid UserId { get; set; }
}