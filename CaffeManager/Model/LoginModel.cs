using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private string _password;

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
