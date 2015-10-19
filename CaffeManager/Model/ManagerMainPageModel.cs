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
    public class ManagerMainPageModel : INotifyPropertyChanged
    {
        private string _name;
        private string _token;

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name)
                    return;

                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private DataServiceCollection<Cashier> _cashiers;

        public DataServiceCollection<Cashier> Cashiers
        {
            get { return _cashiers; }
            set
            {
                if (value == _cashiers)
                    return;

                _cashiers = value;
                OnPropertyChanged("Cashiers");
            }
        }

        private DataServiceCollection<Order> _cashierOrders;

        public DataServiceCollection<Order> CashierOrders
        {
            get { return _cashierOrders; }
            set
            {
                if (value == _cashierOrders)
                    return;

                _cashierOrders = value;
                OnPropertyChanged("CashierOrders");
            }
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
