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
            Model = new LoginModel() {
                Login = "manager1",
                Password = "password",
            };
            DataContext = Model;

            userPassword.Password = Model.Password;
        }

        private async void Signin_Click(object sender, RoutedEventArgs e)
        {
            LoginProgressBar.Visibility = Visibility.Visible;
            await LoginAsync();
            LoginProgressBar.Visibility = Visibility.Collapsed;
        }

        private Task LoginAsync()
        {
            return Task.Factory.StartNew(() => {
                //Pass password from View to Model
                Model.Password = userPassword.Password;

                //Try to signin
                WebApiClient client;
                try
                {
                    client = new WebApiClient(@Model.AppUrl, Model.Login, Model.Password);
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                    return;
                }

                //Initialize DataContext with Url and Access token
                CaffeDataContext.InitializeContext(@Model.AppUrl + @"/CaffeDataService.svc", client.Token);
                this.Dispatcher.Invoke(() => NavigateNextPage(client));
            });
        }

        private void NavigateNextPage(WebApiClient client)
        {
            var userModel = client.GetUserInfo();

            if (userModel.Role == UserRoles.Manager.ToString())
            {
                NavigationService.Navigate(new ManagerMainPage(userModel));
            }
            if (userModel.Role == UserRoles.Cashier.ToString())
            {
                NavigationService.Navigate(new CashierMainPage(userModel));
            }
        }


    }
}
