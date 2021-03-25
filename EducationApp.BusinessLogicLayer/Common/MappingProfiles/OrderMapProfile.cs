using AutoMapper;
using EducationApp.BusinessLogicLayer.Models.Orders;
using EducationApp.DataAccessLayer.Entities;

namespace EducationApp.BusinessLogicLayer.Common.MappingProfiles
{
    public partial class MappingProfiles
    {
        public class OrderMapProfile : Profile
        {
            public OrderMapProfile()
            {
                CreateMap<OrderModel, OrderEntity>().ReverseMap();

                CreateMap<OrderItemModel, OrderItemEntity>().
                    ForMember(destination => destination.PrintingEdition, option => option.Ignore()); ;
                CreateMap<OrderItemEntity, OrderItemModel>();

                CreateMap<PaymentModel, PaymentEntity>().ReverseMap();
            }
        }
    }
}
