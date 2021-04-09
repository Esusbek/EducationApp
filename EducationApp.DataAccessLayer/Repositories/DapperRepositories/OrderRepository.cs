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
using Dapper.Contrib.Extensions;

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

        public List<OrderEntity> Get(Expression<Func<OrderEntity, bool>> filter = null,
            string field = null, bool ascending = true, bool getRemoved = false, int page = Constants.DEFAULTPAGE)
        {
            var orders = base.Get(filter, field, ascending, getRemoved)
                .Skip((page - Constants.DEFAULTPREVIOUSPAGEOFFSET) * Constants.ORDERPAGESIZE)
                .Take(Constants.ORDERPAGESIZE).ToList();
            using (SqlConnection connection = new(_connectionString))
            {
                var users = connection.GetAll<UserEntity>();
                var payments = connection.GetAll<PaymentEntity>();
                foreach (var order in orders)
                {
                    order.User = users.FirstOrDefault(user => user.Id == order.UserId);
                    order.Payment = payments.FirstOrDefault(payment => payment.Id == order.PaymentId);
                }
            }
            return orders;
        }

        public List<OrderEntity> GetAll(Expression<Func<OrderEntity, bool>> filter = null, bool getRemoved = false)
        {
            return base.Get(filter, getRemoved: getRemoved);
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}
