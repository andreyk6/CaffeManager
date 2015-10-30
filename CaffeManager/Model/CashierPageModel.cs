using CaffeManager.CafeManagerServiceReference;
using CaffeManager.Contexts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Services.Client;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaffeManager.Model
{
    class CashierPageModel
    {
        private string _name;
        private string _token;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value == _name)
                {
                    return;
                }
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private List<OrderItem> _currentOrder;

        public List<OrderItem> CurrentOrder
        {
            get
            {
                return _currentOrder;
            }
            set
            {
                if (value == _currentOrder)
                {
                    return;
                }
                _currentOrder = value;
                OnPropertyChanged("CurrentOrder");
            }
        }

        private List<MenuItem> _menu;
        public List<MenuItem> Menu
        {
            get
            {
                return _menu;
            }
            set
            {
                if (value == _menu)
                {
                    return;
                }
                _menu = value;

                OnPropertyChanged("Menu");
            }
        }

        public void AddNewItemToCurrentOrder(int index, decimal count)
        {
            if (HaveCurrentOrder())
            {
                CurrentOrder.Add(new OrderItem() { MenuItem = Menu[index], Count = count, TotalPrice = count*Menu[index].Price });
            }
           // OnPropertyChanged("CurrentOrder");
        }

        public void RemoveItemFromCurrentOreder(MenuItem item)
        {
            if (HaveCurrentOrder() && CurrentOrder.Count > 0)
            {
                foreach (var point in CurrentOrder)
                {
                    if (item.Id != point.Id)
                    {
                        continue;
                    }
                    CurrentOrder.Remove(point);
                }
            }
            OnPropertyChanged("CurrentOrder");
        }

        private bool HaveCurrentOrder()
        {
            if (CurrentOrder == null)
            {
                CurrentOrder = new List<OrderItem>();
            }
            return true;
        }

        private CaffeDbContext _context;
        public CashierPageModel()
        {
           _context = CaffeDataContext.Instance;
            Menu = _context.MenuItems.ToList();
            CurrentOrder = new List<OrderItem>();
        }

        public void Save()
        {
            //Get cashier
            var cashier = (from c in _context.Cashiers.Expand("Orders")
                          where c.Name == Name
                          select c).Single();
            _context.LoadProperty(cashier, "Orders");


            //Create and add Empty order to cashier
            decimal totalPrice = GetTotalPrice(CurrentOrder);
            var currentDateTime = DateTime.Now;
            var order = Order.CreateOrder(0, currentDateTime, cashier.Id, totalPrice, DateTime.Now.Date);
            _context.AddRelatedObject(cashier, "Orders", order);
            _context.SetLink(order, "Cashier", cashier);
            cashier.Orders.Add(order);
            order.Cashier = cashier;
            
            //Save changes
            _context.SaveChanges();

            //Get order from db (for id)

            _context.LoadProperty(order, "OrderItems");

            foreach(var orderItem in CurrentOrder)
            {
                var orderItm = OrderItem.CreateOrderItem(0, orderItem.Count, orderItem.MenuItemId, order.Id, orderItem.TotalPrice);
                _context.AddRelatedObject(order, "OrderItems", orderItm);
                _context.SetLink(orderItm, "Order", order);
                order.OrderItems.Add(orderItm);
                orderItem.Order = order;
            }

            _context.SaveChanges();
        }

        private decimal GetTotalPrice(List<OrderItem> currentOrder)
        {
            decimal totalPrice = 0;
            foreach (var orderItem in currentOrder)
            {
                totalPrice += orderItem.TotalPrice;
            }
            return totalPrice;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
