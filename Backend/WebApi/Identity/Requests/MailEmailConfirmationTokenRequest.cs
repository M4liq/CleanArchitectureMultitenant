using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace WebApi.Identity.Requests;

public class MailEmailConfirmationTokenRequest
{
    [EmailAddress]
    [JsonProperty("email")]
    public string Email { get; set; }
}