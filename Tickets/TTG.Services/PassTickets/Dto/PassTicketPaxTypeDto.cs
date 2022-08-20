using Newtonsoft.Json;

namespace Moonshine.Services.PassTickets.Dto;

public class PassTicketPaxTypeDto
{
    [JsonProperty("tn")]
    public string TypeName { get; set; }
    
    [JsonProperty("pavid")]
    public int ProductAttributeValueId { get; set; }
    
    [JsonProperty("q")]
    public int Quantity { get; set; }
}