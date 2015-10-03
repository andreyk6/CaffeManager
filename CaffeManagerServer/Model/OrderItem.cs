using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaffeManagerServer.Model
{
    public class OrderItem
    {
        public int Id { get; set; }
        public decimal Count { get; set; }
        public virtual MenuItem MenuItem { get; set; }
        public virtual Order Order { get; set; }

        public decimal TotalPrice ()
        {
            return MenuItem.Price * Count;
        }
    }
}