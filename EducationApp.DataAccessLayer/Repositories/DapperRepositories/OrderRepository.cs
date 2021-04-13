using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repositories.Base;
using EducationApp.DataAccessLayer.Repositories.Interfaces;
using EducationApp.Shared.Constants;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Dapper;
using EducationApp.DataAccessLayer.Extensions;
using EducationApp.DataAccessLayer.FilterModels;
using EducationApp.Shared.Enums;

namespace EducationApp.DataAccessLayer.Repositories.DapperRepositories
{
    public class OrderRepository : BaseDapperRepository<OrderEntity>, IOrderRepository
    {
        private readonly string _connectionString;
        public OrderRepository(IConfiguration config)
            : base(config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public List<OrderEntity> Get(OrderFilterModel orderFilter = null,
            string field = null, bool ascending = true, bool getRemoved = false, int page = Constants.DEFAULTPAGE)
        {
            string sql = "select o.*, u.* from Orders o inner join AspNetUsers u on o.UserId=u.Id";
            using (SqlConnection connection = new(_connectionString))
            {
                Expression<Func<OrderEntity, bool>> filter = null;
                if (orderFilter is not null)
                {
                    filter = order => ((orderFilter.GetPaid && order.Status == Enums.OrderStatusType.Paid)
                    || (orderFilter.GetUnpaid && order.Status == Enums.OrderStatusType.Unpaid)) &&
                    (string.IsNullOrWhiteSpace(orderFilter.UserId) || order.UserId == orderFilter.UserId) &&
                    (orderFilter.PaymentId == default || order.PaymentId == orderFilter.PaymentId);
                }
                var orders = connection.Query<OrderEntity, UserEntity, OrderEntity>(sql, (OrderEntity, UserEntity) =>
                {
                    OrderEntity.User = UserEntity;
                    return OrderEntity;
                }).AsQueryable();
                if (!getRemoved)
                {
                    orders = orders.Where(entity => entity.IsRemoved == false);
                }
                if (filter is not null)
                {
                    orders = orders.Where(filter);
                }
                if (field is not null)
                {
                    orders = orders.OrderBy(field, ascending);
                }
                return orders
                .Skip((page - Constants.DEFAULTPREVIOUSPAGEOFFSET) * Constants.ORDERPAGESIZE)
                .Take(Constants.ORDERPAGESIZE).ToList();
            }
            
        }

        public List<OrderEntity> GetAll(OrderFilterModel orderFilter = null, bool getRemoved = false)
        {
            Expression<Func<OrderEntity, bool>> filter = null;
            if (orderFilter is not null)
            {
                filter = order => ((orderFilter.GetPaid && order.Status == Enums.OrderStatusType.Paid)
                || (orderFilter.GetUnpaid && order.Status == Enums.OrderStatusType.Unpaid)) &&
                (string.IsNullOrWhiteSpace(orderFilter.UserId) || order.UserId == orderFilter.UserId) &&
                (orderFilter.PaymentId == default || order.PaymentId == orderFilter.PaymentId);
            }
            string sql = "select o.*, u.* from Orders o inner join AspNetUsers u on o.UserId=u.Id";
            using (SqlConnection connection = new(_connectionString))
            {
                var orders = connection.Query<OrderEntity, UserEntity, OrderEntity>(sql, (OrderEntity, UserEntity) =>
                {
                    OrderEntity.User = UserEntity;
                    return OrderEntity;
                }).AsQueryable();
                if (!getRemoved)
                {
                    orders = orders.Where(entity => entity.IsRemoved == false);
                }
                if (filter is not null)
                {
                    orders = orders.Where(filter);
                }
                return orders.ToList();
            }
        }

        public void SaveChanges()
        {
            return;
        }
    }
}
