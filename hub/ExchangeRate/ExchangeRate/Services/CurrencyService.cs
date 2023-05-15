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

    public async Task<Dictionary<string, decimal>> GetCurrencyRatesAsync(string baseCurrency, params string[] targetCurrencies)
    {
        var response = await _httpClient.GetAsync($"https://v6.exchangerate-api.com/v6/048b1184edd6ffa05b4b8de7/latest/{baseCurrency}");

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeObject<ExchangeRateResponse>(content);

            if (json != null && json.Rates != null)
            {
                var result = new Dictionary<string, decimal>();

                foreach (var targetCurrency in targetCurrencies)
                {
                    if (json.Rates.TryGetValue(targetCurrency, out var rate))
                    {
                        result[targetCurrency] = 1 / rate;
                    }
                    else
                    {
                        throw new Exception($"Döviz kuru bilgisi alýnamadý: {targetCurrency}");
                    }
                }

                return result;
            }
        }

        throw new Exception("Döviz kuru bilgisi alýnamadý.");
    }


}
