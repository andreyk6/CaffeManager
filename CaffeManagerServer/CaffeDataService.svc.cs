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
            config.SetEntitySetAccessRule("*", EntitySetRights.All);
            config.SetServiceOperationAccessRule("*", ServiceOperationRights.All);
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V3;
        }

        [WebGet]
        public IQueryable<CashierStatsModel> GetCashierStats(int cashierId, int statsPeriod)
        {
            switch (statsPeriod)
            {
                case (int)StatsPeriod.Day:
                    return GetCashierDayStats(cashierId);
                case (int)StatsPeriod.Month:
                    return GetCashierMonthStats(cashierId);
                case (int)StatsPeriod.Year:
                    return GetCashierYearStats(cashierId);
                default:
                    //Return empty stats list
                    return new List<CashierDayStatsModel>().AsQueryable();
            }
        }

        private IQueryable<CashierDayStatsModel> GetCashierDayStats(int cashierId)
        {
            //Create instanse of stats object to access date provider
            var queryByDate = from order in CurrentDataSource.Orders
                              where order.CashierId == cashierId
                              group order by order.Date into newGroup
                              orderby newGroup.Key
                              select newGroup;
            return GroupCashierStats<CashierDayStatsModel, DateTime>(queryByDate);
        }
        private IQueryable<CashierMonthStatsModel> GetCashierMonthStats(int cashierId)
        {
            //Create instanse of stats object to access date provider
            var queryByDate = from order in CurrentDataSource.Orders
                              where order.CashierId == cashierId
                              group order by new { order.Date.Year, order.Date.Month } into newGroup
                              orderby newGroup.Key
                              select newGroup;
            return GroupCashierStats<CashierMonthStatsModel, object>(queryByDate);
        }
        private IQueryable<CashierYearStatsModel> GetCashierYearStats(int cashierId)
        {
            //Create instanse of stats object to access date provider
            var queryByDate = from order in CurrentDataSource.Orders
                              where order.CashierId == cashierId
                              group order by order.Date.Year into newGroup
                              orderby newGroup.Key
                              select newGroup;
            return GroupCashierStats<CashierYearStatsModel, int>(queryByDate);
        }
        private IQueryable<T> GroupCashierStats<T,T2>(IOrderedQueryable<IGrouping<T2, Enitites.Order>> queryByDate) where T:CashierStatsModel
        {
            if (queryByDate == null)
            {
                throw new KeyNotFoundException();
            }

            var stats = new List<T>();
            int globalId = 0;

            foreach (var dateGroup in queryByDate)
            {
                var dailyStats = (T)Activator.CreateInstance(typeof(T), null);
                dailyStats.TimeCaption = dailyStats.DateTimeToString(dateGroup.First().Date);

                //Set unique Id
                globalId++;
                dailyStats.Id = globalId;

                foreach (var order in dateGroup)
                    dailyStats.Amount += GetOrderTotalPrice(order.Id);

                stats.Add(dailyStats);
            }

            return stats.AsQueryable();
        }
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
