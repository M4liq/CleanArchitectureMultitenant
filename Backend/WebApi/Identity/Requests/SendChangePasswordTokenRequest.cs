using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace WebApi.Identity.Requests;

public class SendChangePasswordTokenRequest
{
    [EmailAddress]
    [JsonProperty("email")]
    public string Email { get; set; }
}