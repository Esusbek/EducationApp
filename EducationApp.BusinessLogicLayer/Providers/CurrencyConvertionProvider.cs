using Api.Forex.Sharp;
using EducationApp.Shared.Configs;
using EducationApp.Shared.Constants;
using Microsoft.Extensions.Options;
using System;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.BusinessLogicLayer.Providers
{
    public class CurrencyConvertionProvider : Interfaces.ICurrencyConvertionProvider
    {
        private readonly string _apiKey;
        private Api.Forex.Sharp.Models.ApiForexRates _rates;
        private DateTime _lastFetched;
        public CurrencyConvertionProvider(IOptions<CurrencyConvertConfig> config)
        {
            _apiKey = config.Value.ApiKey;
            _lastFetched = DateTime.MinValue;

        }

        public async Task<decimal> ConvertAsync(string fromCurrency, string toCurrency, decimal amount)
        {
            await GetRatesAsync();
            if (fromCurrency.Equals(toCurrency))
            {
                return amount;
            }
            return _rates.Convert(fromCurrency, toCurrency, amount);
        }

        private async Task GetRatesAsync()
        {
            if ((DateTime.UtcNow - _lastFetched).TotalDays > Constants.DEFAULTDAYSPERRATEREFRESH)
            {
                _rates = await ApiForex.GetRate(_apiKey);
                _lastFetched = DateTime.UtcNow;
            }
        }
    }
}
