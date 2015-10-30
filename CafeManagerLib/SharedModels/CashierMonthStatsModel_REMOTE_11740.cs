using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManagerLib.SharedModels
{
    public class CashierMonthStatsModel : CashierStatsModel
    {
        public override string DateTimeToString(DateTime dt)
        {
            return dt.ToString("0:yyyy/mm");
        }
    }
}
