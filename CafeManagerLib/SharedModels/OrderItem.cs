using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaffeManagerLib.SharedModels
{
    public class OrderItem
    {
        public int Id { get; set; }

        private decimal _count;

        public decimal Count
        {
            get
            {
                return _count;
            }
            set
            {
                if (MenuItem != null)
                    TotalPrice = MenuItem.Price * value;
                _count = value;
            }
        }

        private OrderItem()
        {
            MenuItem = new MenuItem();
        }

        public OrderItem(MenuItem menuItem)
        {
            MenuItem = menuItem;
        }

        public int MenuItemId { get; set; }
        public virtual MenuItem MenuItem { get; set; }

        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        public decimal TotalPrice { get; private set; }
    }
}