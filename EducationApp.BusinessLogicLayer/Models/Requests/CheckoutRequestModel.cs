namespace EducationApp.BusinessLogicLayer.Models.Requests
{
    public class CheckoutRequestModel
    {
        public Orders.OrderModel Order { get; set; }
        public Users.UserModel User { get; set; }
    }
}
