﻿using CaffeManager.CafeManagerServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CaffeManager.View
{
    /// <summary>
    /// Логика взаимодействия для AddManagerWindow.xaml
    /// </summary>
    public partial class AddManagerWindow: Window
    {
        public AddManagerWindow(Manager cashier)
        {
            InitializeComponent();
            cashier.Name = "";
            cashier.Login = "";
            cashier.Password = "";
            cashier.Role = "Manager";
            DataContext = cashier;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
