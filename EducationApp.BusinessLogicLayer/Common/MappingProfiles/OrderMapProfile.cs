using AutoMapper;
using EducationApp.BusinessLogicLayer.Models.Orders;
using EducationApp.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.BusinessLogicLayer.Common.MappingProfiles
{
    public partial class MappingProfiles
    {
        public class OrderMapProfile : Profile
        {
            public OrderMapProfile()
            {
                CreateMap<OrderModel, OrderEntity>();
                CreateMap<OrderEntity, OrderModel>();

                CreateMap<OrderItemModel, OrderItemEntity>();
                CreateMap<OrderItemEntity, OrderItemModel>();

                CreateMap<PaymentModel, PaymentEntity>();
                CreateMap<PaymentEntity, PaymentModel>();
            }
        }
    }
}
