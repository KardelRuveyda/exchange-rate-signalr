using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class CurrencyHub : Hub
{
    public async Task UpdateCurrencyRate(string currency, decimal rate)
    {
        await Clients.All.SendAsync("ReceiveCurrencyRate", currency, rate);
    }
}
