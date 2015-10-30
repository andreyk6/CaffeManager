using CaffeManager.CafeManagerServiceReference;
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
    /// Логика взаимодействия для CashierAddWindow.xaml
    /// </summary>
    public partial class CashierAddWindow : Window
    {
        public CashierAddWindow(Cashier cashier)
        {
            InitializeComponent();
            cashier.Name = "Cashier 3";
            cashier.Login = "cashier3";
            cashier.Password = "cashier3pass";
            cashier.Role = "Cashier";
            DataContext = cashier;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
