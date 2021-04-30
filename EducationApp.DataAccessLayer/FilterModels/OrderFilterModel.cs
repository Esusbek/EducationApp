namespace EducationApp.DataAccessLayer.FilterModels
{
    public class OrderFilterModel
    {

        public bool IsPaid { get; set; }
        public bool IsUnpaid { get; set; }
        public string UserId { get; set; }
        public int? PaymentId { get; set; }
        public OrderFilterModel()
        {
            IsPaid = true;
            IsUnpaid = true;
            UserId = string.Empty;
        }
    }
}
