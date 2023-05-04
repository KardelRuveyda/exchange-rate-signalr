
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

public class ExchangeRateResponse
{
    [JsonProperty("base_code")]
    public string Base { get; set; }

    [JsonProperty("conversion_rates")]
    public Dictionary<string, decimal> Rates { get; set; }
}
