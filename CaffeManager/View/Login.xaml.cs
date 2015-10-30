﻿using CaffeManager.Contexts;
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
                Password = "password"
            };
            DataContext = Model;

            userPassword.Password = Model.Password;
        }

        private void Signin_Click(object sender, RoutedEventArgs e)
        {
            //Pass password from View to Model
            Model.Password = userPassword.Password;

            //Try to signin
            WebApiClient client;
            try
            {
                client = new WebApiClient(@"http://localhost:58156", Model.Login, Model.Password);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                return;
            }

            //Initialize DataContext with Url and Access token
            CaffeDataContext.InitializeContext(@"http://localhost:58156/CaffeDataService.svc", client.Token);

            NavigateNextPage(client);
        }

        private void NavigateNextPage(WebApiClient client)
        {
            var userModel = client.GetUserInfo();

            if (userModel.Role == UserRoles.Manager.ToString())
            {
                NavigationService.Navigate(userModel);
            }
            if (userModel.Role == UserRoles.Cashier.ToString())
            {
                NavigationService.Navigate(new CashierMainPage());
            }
        }


    }
}
