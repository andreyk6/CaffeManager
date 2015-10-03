using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaffeManagerServer.Model
{
    public class OrderItem
    {
        public int Id { get; set; }
        public decimal Count
        {
            get
            {
                return Count;
            }
            set
            {
                TotalPrice = MenuItem.Price * value;
                Count = value;
            }
        }

        public int MenuItemId { get; set; }
        public virtual MenuItem MenuItem { get; set; }

        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        public decimal TotalPrice { get; private set; }
    }
}