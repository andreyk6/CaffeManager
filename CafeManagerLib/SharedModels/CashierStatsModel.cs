using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManagerLib.SharedModels
{
    public abstract class CashierStatsModel
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }

        public string TimeCaption { get; set; }

        public abstract string DateTimeToString(DateTime dt);
    }
}
