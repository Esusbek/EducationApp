﻿using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.FilterModels;
using EducationApp.DataAccessLayer.Repositories.Base;
using EducationApp.DataAccessLayer.Repositories.Interfaces;
using EducationApp.Shared.Constants;
using EducationApp.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EducationApp.DataAccessLayer.Repositories.EFRepositories
{
    public class OrderRepository : BaseRepository<OrderEntity>, IOrderRepository
    {
        public OrderRepository(ApplicationContext dbContext)
            : base(dbContext)
        {
        }
        public List<OrderEntity> Get(OrderFilterModel orderFilter = null, string field = null, bool ascending = true, bool getRemoved = false, int page = Constants.DEFAULTPAGE)
        {
            page = page < Constants.DEFAULTPAGE ? Constants.DEFAULTPAGE : page;
            Expression<Func<OrderEntity, bool>> filter = null;
            if (orderFilter is not null)
            {
                filter = order => ((orderFilter.IsPaid && order.Status == Enums.OrderStatusType.Paid)
                || (orderFilter.IsUnpaid && order.Status == Enums.OrderStatusType.Unpaid)) &&
                (string.IsNullOrWhiteSpace(orderFilter.UserId) || order.UserId == orderFilter.UserId) &&
                (orderFilter.PaymentId != null || order.PaymentId == orderFilter.PaymentId);
            }
            return base.Get(filter, field, ascending, getRemoved)
                .Skip((page - Constants.DEFAULTPREVIOUSPAGEOFFSET) * Constants.ORDERPAGESIZE)
                .Take(Constants.ORDERPAGESIZE).ToList();
        }
        public List<OrderEntity> GetAll(OrderFilterModel orderFilter = null, bool getRemoved = false)
        {
            Expression<Func<OrderEntity, bool>> filter = null;
            if (orderFilter is not null)
            {
                filter = order => ((orderFilter.IsPaid && order.Status == Enums.OrderStatusType.Paid)
                || (orderFilter.IsUnpaid && order.Status == Enums.OrderStatusType.Unpaid)) &&
                (string.IsNullOrWhiteSpace(orderFilter.UserId) || order.UserId == orderFilter.UserId) &&
                (orderFilter.PaymentId != null || order.PaymentId == orderFilter.PaymentId);
            }
            return base.Get(filter, getRemoved: getRemoved).ToList();
        }
        public int GetCount(OrderFilterModel orderFilter, bool getRemoved = false)
        {
            Expression<Func<OrderEntity, bool>> filter = null;
            if (orderFilter is not null)
            {
                filter = order => ((orderFilter.IsPaid && order.Status == Enums.OrderStatusType.Paid)
                || (orderFilter.IsUnpaid && order.Status == Enums.OrderStatusType.Unpaid)) &&
                (string.IsNullOrWhiteSpace(orderFilter.UserId) || order.UserId == orderFilter.UserId) &&
                (orderFilter.PaymentId != null || order.PaymentId == orderFilter.PaymentId);
            }
            return base.Get(filter, getRemoved: getRemoved).Count();
        }
    }
}
