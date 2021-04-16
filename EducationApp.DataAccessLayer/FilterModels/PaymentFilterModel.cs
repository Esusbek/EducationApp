namespace EducationApp.DataAccessLayer.FilterModels
{
    public class PaymentFilterModel
    {
        public string TransactionId { get; set; }
        public PaymentFilterModel()
        {
            TransactionId = string.Empty;
        }
    }
}
