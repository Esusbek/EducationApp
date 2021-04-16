using Dapper;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Extensions;
using EducationApp.DataAccessLayer.FilterModels;
using EducationApp.DataAccessLayer.Repositories.Base;
using EducationApp.DataAccessLayer.Repositories.Interfaces;
using EducationApp.Shared.Constants;
using EducationApp.Shared.Enums;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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
            string field = Constants.DEFAULTORDERSORT, bool ascending = true, bool getRemoved = false, int page = Constants.DEFAULTPAGE)
        {
            page = page < Constants.DEFAULTPAGE ? Constants.DEFAULTPAGE : page;
            string sortOrder = ascending ? "asc" : "desc";
            string paidFilter = orderFilter.GetPaid ? $"o.Status={(int)Enums.OrderStatusType.Paid}" : string.Empty;
            string unpaidFilter = orderFilter.GetUnpaid ? $"o.Status={(int)Enums.OrderStatusType.Unpaid}" : string.Empty;
            string paymentIdFilter = !(orderFilter.PaymentId == default) ? $"o.PaymentId={orderFilter.PaymentId}" : string.Empty;
            string userIdFilter = !string.IsNullOrWhiteSpace(orderFilter.UserId) ? $"o.UserId={orderFilter.UserId}" : string.Empty;
            string removedFilter = getRemoved ? "1" : "0";
            string filterString = $"where o.IsRemoved={removedFilter}";
            if (!string.IsNullOrWhiteSpace(paidFilter) || !string.IsNullOrWhiteSpace(unpaidFilter))
            {
                filterString += " and (";
            }
            if (!string.IsNullOrWhiteSpace(paidFilter))
            {
                filterString += $" {paidFilter} ";
            }
            if(!string.IsNullOrWhiteSpace(paidFilter) && !string.IsNullOrWhiteSpace(unpaidFilter))
            {
                filterString += " or ";
            }
            if (!string.IsNullOrWhiteSpace(unpaidFilter))
            {
                filterString += $" {unpaidFilter} ";
            }
            if (!string.IsNullOrWhiteSpace(paidFilter) || !string.IsNullOrWhiteSpace(unpaidFilter))
            {
                filterString += ")";
            }
            if(!string.IsNullOrWhiteSpace(userIdFilter) || !string.IsNullOrWhiteSpace(paymentIdFilter))
            {
                filterString += " and ";
            }
            if (!string.IsNullOrWhiteSpace(userIdFilter))
            {
                filterString += $"{userIdFilter}";
            }
            if (!string.IsNullOrWhiteSpace(paymentIdFilter))
            {
                filterString += " and ";
            }
            if (!string.IsNullOrWhiteSpace(paymentIdFilter))
            {
                filterString += $"{paymentIdFilter}";
            }
            string sql = $"select o.*, u.* from Orders o inner join AspNetUsers u on o.UserId=u.Id {filterString} order by {field} {sortOrder} offset {(page - Constants.DEFAULTPREVIOUSPAGEOFFSET) * Constants.ORDERPAGESIZE} rows fetch next {Constants.ORDERPAGESIZE} rows only";
            using SqlConnection connection = new(_connectionString);
            var orders = connection.Query<OrderEntity, UserEntity, OrderEntity>(sql, (OrderEntity, UserEntity) =>
            {
                OrderEntity.User = UserEntity;
                return OrderEntity;
            });
            return orders.ToList();

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
            using SqlConnection connection = new(_connectionString);
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

        public void SaveChanges()
        {
            return;
        }
    }
}
