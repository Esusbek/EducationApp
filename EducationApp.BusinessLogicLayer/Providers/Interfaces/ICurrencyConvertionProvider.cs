using System.Threading.Tasks;

namespace EducationApp.BusinessLogicLayer.Providers.Interfaces
{
    public interface ICurrencyConvertionProvider
    {
        public Task<decimal> ConvertAsync(string fromCurrency, string toCurrency, decimal amount);
    }
}
