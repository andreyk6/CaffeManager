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
    public class ManagerMainPageModel : INotifyPropertyChanged
    {
        private CaffeDataContext _context;
        private string _name;
        private string _token;
        private IEnumerable<CashierStatsModel> _cashierStats;
        private IEnumerable<Cashier> _cashiers;
        private int _statsPeriod;

        public ManagerMainPageModel()
        {
        }
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

        public IEnumerable<Cashier> Cashiers
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

        public int StatsPeriod
        {
            get
            {
                return _statsPeriod;
            }
            set
            {
                if (value == _statsPeriod)
                    return;

                _statsPeriod = value;
                OnPropertyChanged("SatsPeriod");
            }
        }

        public IEnumerable<CashierStatsModel> CashierStats
        {
            get { return _cashierStats; }
            set
            {
                if (value == _cashierStats)
                    return;

                _cashierStats = value;
                OnPropertyChanged("CashierStats");
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
