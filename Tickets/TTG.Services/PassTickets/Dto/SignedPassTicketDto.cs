using Newtonsoft.Json;

namespace Moonshine.Services.PassTickets.Dto;

public class SignedPassTicketDto
{
    [JsonProperty("p")]
    public PassTicketDto Payload { get; set; }
    
    [JsonProperty("sig")]
    public string Base64Signature { get; set; }
}