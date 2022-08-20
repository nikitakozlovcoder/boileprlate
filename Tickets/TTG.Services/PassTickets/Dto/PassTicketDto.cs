using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Moonshine.Services.PassTickets.Dto;

public class PassTicketDto
{
    [JsonProperty("oid")]
    public int OrderItemId { get; set; }
    
    [JsonProperty("pt")]
    public string ProductSku { get; set; }
    
    [JsonProperty("pisd")]
    public DateTime ProductInstanceStartDate { get; set; }
    
    [JsonProperty("cfn")]
    public string CustomerFirstName { get; set; }
    
    [JsonProperty("cln")]
    public string CustomerLastName { get; set; }
    
    [JsonProperty("g")]
    public List<PassTicketPaxTypeDto> Guests { get; set; } = new ();
}