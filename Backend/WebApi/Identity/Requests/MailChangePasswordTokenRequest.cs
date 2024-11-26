using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace WebApi.Identity.Requests;

public class MailChangePasswordTokenRequest
{
    [EmailAddress]
    [JsonProperty("email")]
    public string Email { get; set; }
}