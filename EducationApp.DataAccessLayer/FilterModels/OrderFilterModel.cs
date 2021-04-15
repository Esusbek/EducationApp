namespace EducationApp.DataAccessLayer.FilterModels
{
    public class OrderFilterModel
    {
        public bool GetPaid { get; set; }
        public bool GetUnpaid { get; set; }
        public string UserId { get; set; }
        public int PaymentId { get; set; }
        public OrderFilterModel()
        {
            GetPaid = true;
            GetUnpaid = true;
            UserId = "";
            PaymentId = default;
        }
    }
}
