using CaffeManager.Contexts;
using CaffeManager.Extension;
using CaffeManager.View;
using CaffeManagerLib.SharedModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CaffeManager.Model
{
    public class LoginModel : INotifyPropertyChanged
    {
        private string _login;

        public string Login
        {
            get { return _login; }
            set
            {
                if (value == _login)
                    return;

                _login = value;
                OnPropertyChanged("Login");
            }
        }


        private bool _commandExecuting;

        public bool CommandExecuting
        {
            get { return _commandExecuting; }
            set
            {
                if (value == _commandExecuting)
                    return;

                _commandExecuting = value;
                OnPropertyChanged("CommandExecuting");
            }
        }
        private string _password;
        private Page _hostPage;

        public string Password
        {
            get { return _password; }
            set
            {
                if (value == _password)
                    return;

                _password = value;
                OnPropertyChanged("Password");
            }
        }

        private string _appUrl;

        public string AppUrl
        {
            get { return _appUrl; }
            set
            {
                if (value == _appUrl)
                    return;

                _appUrl = value;
                OnPropertyChanged("AppUrl");
            }
        }

        public LoginModel(Page page)
        {
            _hostPage = page;


            CommandExecuting = false;

            _loginCommand = new AsyncCommand(async () =>
            {
               await LoginAsync();
            });
        }

        private Task LoginAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                CommandExecuting = true;
                //Try to signin
                WebApiClient client;
                try
                {
                    client = new WebApiClient(@AppUrl, Login, Password);
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                    return false;
                }

                try
                {
                    //Initialize DataContext with Url and Access token
                    CaffeDataContext.InitializeContext(@AppUrl + @"/CaffeDataService.svc", client.Token);

                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                    return false;
                }

                var userModel = client.GetUserInfo();

                if (userModel.Role == UserRoles.Manager.ToString())
                {
                    _hostPage.Dispatcher.Invoke(() => _hostPage.NavigationService.Navigate(new ManagerMainPage(userModel)));
                }
                if (userModel.Role == UserRoles.Cashier.ToString())
                {
                    _hostPage.Dispatcher.Invoke(() => _hostPage.NavigationService.Navigate(new CashierMainPage(userModel)));
                }
                if (userModel.Role == UserRoles.SuperUser.ToString())
                {
                    _hostPage.Dispatcher.Invoke(() => _hostPage.NavigationService.Navigate(new SuperuserMainPage(userModel)));
                }
                return true;
            }).ContinueWith((result) => CommandExecuting = false);
        }

        private AsyncCommand _loginCommand;
        public IAsyncCommand LoginCommand
        {
            get
            {
                return _loginCommand;
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
