using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace WebApi.Identity.Requests;

public class UserRegistrationRequest
{
    [EmailAddress]
    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("password")]
    public string Password { get; set; }
}