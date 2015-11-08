using CaffeManager.Contexts;
using CaffeManager.Model;
using CaffeManagerLib.SharedModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CaffeManager.View
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public LoginModel Model { get; set; }

        public Login()
        {
            InitializeComponent();
            Model = new LoginModel(this) {
                Login = "manager1",
                Password = "password",
            };
            DataContext = Model;

            userPassword.Password = Model.Password;
        }

        private void userPassword_KeyUp(object sender, KeyEventArgs e)
        {
            //Implement Password binding
            Model.Password = userPassword.Password;
        }
    }
}
