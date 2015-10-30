using CaffeManager.CafeManagerServiceReference;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaffeManager.Extension
{
    public static class CaffeDataServiceExtension
    {
        public static IEnumerable<CashierStatsModel> GetCashierStats(this CaffeDbContext context, int cashierId, int statsPeriod)
        {
            //Uri u = new Uri(string.Format("{0}/GetCashierStats()?cashierId={1}&statsPeriod={2}",
            //         context.BaseUri, cashierId, statsPeriod), UriKind.RelativeOrAbsolute);

            //var data = context.Execute<CashierStatsModel>(u);

            var data = context.CreateQuery<CashierStatsModel>("GetCashierStats")
                .AddQueryOption("cashierId", cashierId)
                .AddQueryOption("statsPeriod", statsPeriod);

            return data.ToList();
        }
    }
}
