
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

[Route("api/[controller]")]
[ApiController]
public class CurrencyController : ControllerBase
{
    private readonly CurrencyService _currencyService;
    private readonly IHubContext<CurrencyHub> _currencyHub;

    public CurrencyController(CurrencyService currencyService, IHubContext<CurrencyHub> currencyHub)
    {
        _currencyService = currencyService;
        _currencyHub = currencyHub;
    }

    [HttpGet("update")]
    public async Task<IActionResult> UpdateCurrencyRate()
    {
        var baseCurrency = "USD";
        var targetCurrency = "TRY";
        var rate = await _currencyService.GetCurrencyRateAsync(baseCurrency, targetCurrency);
        await _currencyHub.Clients.All.SendAsync("ReceiveCurrencyRate", targetCurrency, rate);
        return Ok();
    }
}
