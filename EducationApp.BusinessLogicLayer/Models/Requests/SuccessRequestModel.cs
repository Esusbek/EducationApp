namespace EducationApp.BusinessLogicLayer.Models.Requests
{
    public class SuccessRequestModel
    {
        public Orders.OrderModel Order { get; set; }
        public string TransactionId { get; set; }
    }
}
