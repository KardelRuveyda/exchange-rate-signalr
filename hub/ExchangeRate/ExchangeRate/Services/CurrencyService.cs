using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class CurrencyService
{
    private readonly HttpClient _httpClient;

    public CurrencyService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<decimal> GetCurrencyRateAsync(string baseCurrency, string targetCurrency)
    {
        var response = await _httpClient.GetAsync($"https://v6.exchangerate-api.com/v6/048b1184edd6ffa05b4b8de7/latest/{baseCurrency}");

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeObject<ExchangeRateResponse>(content);

            if (json != null && json.Rates.TryGetValue(targetCurrency, out var rate))
            {
                return rate;
            }
        }

        throw new Exception("Döviz kuru bilgisi alýnamadý.");
    }

}
