using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CaffeManagerLib.SharedModels
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }

        public int CashierId { get; set; }
        public virtual Cashier Cashier { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }

        public decimal TotalPrice
        {
            get
            {
                decimal total = 0;
                foreach (var orderItem in OrderItems)
                {
                    total += orderItem.TotalPrice;
                }
                return total;
            }
            private set { }
        }

        public DateTime Date
        {
            get
            {
                return DateTime.Date;
            }
            private set { }
        } 
        public Order()
        {
            DateTime = DateTime.Now;
        }
    }
}