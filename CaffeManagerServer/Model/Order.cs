using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaffeManagerServer.Model
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public int CashierId { get; set; }
        public virtual Cashier Cashier { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }

        public Order()
        {
            Date = DateTime.Now;
        }
    }
}