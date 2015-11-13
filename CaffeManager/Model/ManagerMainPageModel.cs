using CaffeManager.CafeManagerServiceReference;
using CaffeManager.Contexts;
using CaffeManager.Extension;
using CaffeManager.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Services.Client;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CaffeManager.Model
{
    public class ManagerMainPageModel : INotifyPropertyChanged
    {
        private CaffeDbContext _context;
        private string _name;
        private string _token;
        private IEnumerable<CashierStatsModel> _cashierStats;
        private IEnumerable<Cashier> _cashiers;
        private int _statsPeriod;
        private int _sellectedCashierId;
        private string _infoMessage;

        public ManagerMainPageModel()
        {
            _context = CaffeDataContext.Instance;
            StatsPeriod = 0;
            Cashiers = new List<Cashier>();
            CashierStats = new List<CashierStatsModel>();
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

        public int SellectedCashierId
        {
            get
            {
                return _sellectedCashierId;
            }
            set
            {
                if (value == _sellectedCashierId)
                    return;

                _sellectedCashierId = value;
                OnPropertyChanged("SellectedCashierId");
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
                UpdateCashierStats();
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

        public string InfoMessage
        {
            get
            {
                return _infoMessage;
            }
            set
            {
                if (value == _infoMessage)
                    return;

                _infoMessage = value;
                OnPropertyChanged("InfoMessage");
            }
        }
        
        public void UpdateCashiersList()
        {
            try
            {
                var manager = _context.Managers.Where(m => m.Name == Name).FirstOrDefault();

                var cashiersQuery = from cashier in _context.Cashiers
                                    where cashier.ManagerId == manager.Id
                                    select cashier;

                Cashiers = new DataServiceCollection<Cashier>(cashiersQuery);
            }
            catch (DataServiceQueryException ex)
            {
                InfoMessage = "The query could not be completed:\n" + ex.ToString();
            }
            catch (InvalidOperationException ex)
            {
                InfoMessage = "The following error occurred:\n" + ex.ToString();
            }
        }

        public void UpdateCashierStats()
        {
            CashierStats = CaffeDataServiceExtension.GetCashierStats(_context, SellectedCashierId, StatsPeriod).ToList();
        }

        public void AddCashier()
        {
            var manager = (from m in _context.Managers.Expand("Cashiers")
                           where m.Name == Name
                           select m).Single();

            _context.LoadProperty(manager, "Cashiers");
            var newCashier = Cashier.CreateCashier(0, manager.Id);

            var result = new AddCashierWindow(newCashier);
            if (result.ShowDialog() == true)
            {
                try
                {
                    _context.AddRelatedObject(manager, "Cashiers", newCashier);
                    _context.SetLink(newCashier, "Manager", manager);

                    manager.Cashiers.Add(newCashier);
                    newCashier.Manager = manager;

                    // Send the insert to the data service.
                    DataServiceResponse response = _context.SaveChanges();

                    UpdateCashiersList();
                    InfoMessage = "Cashier added";
                }
                catch (DataServiceRequestException ex)
                {
                    InfoMessage = ex.Message;
                }
            }
            else
            {
                InfoMessage = "Error";
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
