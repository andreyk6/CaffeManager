using CaffeManager.CafeManagerServiceReference;
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

        private IEnumerable<MenuItem> _menu;
        public IEnumerable<MenuItem> Menu
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

        public void AddNewItemToCurrentOrder(MenuItem newItem, decimal count)
        {
            if (HaveCurrentOrder())
            {
                CurrentOrder.Add(new OrderItem(){ MenuItem = newItem, Count = count });
            }
            foreach (var item in CurrentOrder)
            {
                if (item.Id == newItem.Id)
                {
                    item.Count += count;
                    OnPropertyChanged("CurrentOrder");
                    return;
                }
            }
            CurrentOrder.Add(new OrderItem(){ MenuItem = newItem, Count = count });
            OnPropertyChanged("CurrentOrder");
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
           // _context = CaffeDbContext.Instance;
            Menu = _context.MenuItems.Execute();
            CurrentOrder = new List<OrderItem>();
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
