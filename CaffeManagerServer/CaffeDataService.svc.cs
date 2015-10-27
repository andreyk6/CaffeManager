//------------------------------------------------------------------------------
// <copyright file="WebDataService.svc.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using CafeManagerLib.SharedModels;
using CaffeManagerServer.Context;
using System.Collections.Generic;
using System.Data.Services;
using System.Data.Services.Common;
using System.ServiceModel;
using System.Linq;
using System;
using System.ServiceModel.Web;

namespace CaffeManagerServer
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class CaffeDataService : DataService<CaffeDbContext>
    {
        // This method is called only once to initialize service-wide policies.
        public static void InitializeService(DataServiceConfiguration config)
        {
            config.UseVerboseErrors = true;
            config.RegisterKnownType(typeof(CashierDayStatsModel));
            config.SetEntitySetAccessRule("*", EntitySetRights.AllRead);
            config.SetServiceOperationAccessRule("*", ServiceOperationRights.All);
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V3;
        }

        [WebGet]
        public IQueryable<CashierDayStatsModel> GetCashierDailyStats(int cashierId)
        {
            var queryByDate = from order in CurrentDataSource.Orders
                              group order by order.Date into newGroup
                              orderby newGroup.Key
                              select newGroup;

            if (queryByDate == null)
            {
                throw new KeyNotFoundException();
            }

            var stats = new List<CashierDayStatsModel>();

            foreach (var dateGroup in queryByDate)
            {
                var dailyStats = new CashierDayStatsModel();
                dailyStats.Date = dateGroup.First().Date;

                foreach (var order in dateGroup)
                    dailyStats.Amount += GetOrderTotalPrice(order.Id);

                stats.Add(dailyStats);
            }

            return stats.AsQueryable();
        }

        [WebGet]
        public decimal GetOrderTotalPrice(int orderId)
        {
            var orderItems = CurrentDataSource.OrderItems.Where(oi => oi.OrderId == orderId);
            if (orderItems == null)
            {
                return 0;
            }

            decimal total = 0;
            foreach (var oi in orderItems)
            {
                total += oi.TotalPrice;
            }
            return total;
        }
    }
}
