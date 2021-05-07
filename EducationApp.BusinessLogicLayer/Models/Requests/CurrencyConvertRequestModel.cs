namespace EducationApp.BusinessLogicLayer.Models.Requests
{
    public class CurrencyConvertRequestModel
    {
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public decimal Amount { get; set; }
    }
}
