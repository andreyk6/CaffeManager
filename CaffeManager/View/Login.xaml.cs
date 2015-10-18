using CafeManagerLib.Client;
using CafeManagerLib.ModelShared;
using CaffeManager.Model;
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
        public LoginModel LoginModel { get; set; }
         
        public Login()
        {
            InitializeComponent();
            LoginModel = new LoginModel() {
                Login = "Enter Login or Email",
                Password = "Enter Password"
            };
            DataContext = LoginModel;
        }

        private void Signin_Click(object sender, RoutedEventArgs e)
        {
            WebApiClient client;
            try {
                client = new WebApiClient(@"http://localhost:58156", LoginModel.Login, LoginModel.Password);
            }
            catch(Exception exc)
            {
                MessageBox.Show(exc.Message);
                return;
            }


            var userData = client.GetUserInfo();

            if (userData.Role == UserRoles.Manager.ToString())
            {
                NavigationService.Navigate(new ManagerMainPage());
            }
            if (userData.Role == UserRoles.Cashier.ToString())
            {
                NavigationService.Navigate(new CashierMainPage());
            }
        }


        
    }
}
