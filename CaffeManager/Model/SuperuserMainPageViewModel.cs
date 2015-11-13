using CaffeManager.CafeManagerServiceReference;
using CaffeManager.Contexts;
using CaffeManager.Extension;
using CaffeManager.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Services.Client;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CaffeManager.Model
{
    public class SuperuserMainPageViewModel : INotifyPropertyChanged
    {
        private CaffeDbContext _context;

        private Superuser _model;
        public Superuser Model
        {
            get
            {
                return _model;
            }
            set {
                if (value == _model)
                    return;

                _model = value;
                OnPropertyChanged("Model");
            }
        }

        private string _infoMessage;
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

        private Manager _selectedManager;
        public Manager SelectedManager
        {
            get
            {
                return _selectedManager;
            }
            set
            {
                if (value == _selectedManager)
                    return;

                _selectedManager = value;

                //Update Manager View
                ManagerMainPage = new ManagerMainPage(SelectedManager);

                OnPropertyChanged("SelectedManager");
            }
        } 

        public ManagerMainPage _managerMainPage;
        public ManagerMainPage ManagerMainPage
        {
            get
            {
                return _managerMainPage;
            }
            set
            {
                if (value == _managerMainPage)
                    return;

                _managerMainPage = value;
                OnPropertyChanged("ManagerMainPage");
            }
        }

        private AsyncCommand _addManagerCommand;
        public IAsyncCommand AddManagerCommand
        {
            get
            {
                return _addManagerCommand;
            }
        }

        public List<Manager> Managers
        {
            get
            {
                return Model.Managers.ToList();
            }
        }

        public SuperuserMainPageViewModel(String superuserName)
        {
            _context = CaffeDataContext.Instance;
            var superuser = _context.Superusers.Expand(m => m.Managers).Where(s => s.Login == superuserName).FirstOrDefault();

            if (superuser != null)
            {
                Model = superuser;
            }
            else
            {
                throw new KeyNotFoundException();
            }

            _addManagerCommand = new AsyncCommand(async () => {
                AddManager();
            });
        }

        public void AddManager()
        {
            //Get user state from server
            var superuser = _context.Superusers
                .Expand(m => m.Managers)
                .Where(s => s.Login == Model.Login)
                .FirstOrDefault();
         

            var newManager = Manager.CreateManager(0, superuser.Id);
            
            var result = new AddManagerWindow(newManager);
            if (result.ShowDialog() == true)
            {
                try
                {
                    _context.AddRelatedObject(superuser, "Managers", newManager);

                    _context.SetLink(newManager, "Superuser", superuser);

                    superuser.Managers.Add(newManager);
                    newManager.Superuser = superuser;

                    // Send the insert to the data service.
                    DataServiceResponse response = _context.SaveChanges();
                    
                    //Update user model state
                    Model = superuser;

                    OnPropertyChanged("Managers");
                    InfoMessage = "Manager added";
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
