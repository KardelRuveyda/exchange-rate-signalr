
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

[Route("api/[controller]")]
[ApiController]
public class CurrencyController : ControllerBase
{
    private readonly CurrencyService _currencyService;
    private readonly IHubContext<CurrencyHub> _currencyHub;
    private Dictionary<string, decimal> _previousRates = new Dictionary<string, decimal>();

    public CurrencyController(CurrencyService currencyService, IHubContext<CurrencyHub> currencyHub)
    {
        _currencyService = currencyService;
        _currencyHub = currencyHub;
    }

    [HttpGet("update")]
    // �rnek API taraf� kodu

    public async Task<IActionResult> UpdateCurrencyRates()
    {
        var baseCurrency = "TRY";
        var targetCurrencies = new[] { "USD", "EUR", "GBP" };

        var currentRates = await _currencyService.GetCurrencyRatesAsync(baseCurrency, targetCurrencies);

        foreach (var targetCurrency in targetCurrencies)
        {
            // �nceki d�viz kuru de�erini elde etmek i�in
            _previousRates.TryGetValue(targetCurrency, out decimal previousRate);

            decimal currentRate = currentRates[targetCurrency];
            decimal change = currentRate - previousRate; // D�viz kuru de�i�imi hesaplan�yor

            await _currencyHub.Clients.All.SendAsync("ReceiveCurrencyRate", $"{baseCurrency}/{targetCurrency}", currentRate, change);

            // �imdiki de�eri �nceki de�er olarak sakla
            _previousRates[targetCurrency] = currentRate;
        }

        return Ok();
    }


}
