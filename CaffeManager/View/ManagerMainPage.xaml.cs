using CaffeManager.CafeManagerServiceReference;
using CaffeManager.Model;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using CaffeManager.Extension;
using CaffeManager.Contexts;

namespace CaffeManager.View
{
    /// <summary>
    /// Логика взаимодействия для ManagerMainPage.xaml
    /// </summary>
    public partial class ManagerMainPage : Page
    {
        public ManagerMainPageModel Model { get; set; }
        CaffeDbContext _context;
        private object userModel;

        public ManagerMainPage(CafeManagerLib.SharedModels.UserClientModel userModel)
        {
            InitializeComponent();

            _context = CaffeDataContext.Instance;

            Model = new ManagerMainPageModel()
            {
                Name = userModel.Name,
            };

            DataContext = Model;

            Model.UpdateCashiersList();
        }

        public ManagerMainPage(object userModel)
        {
            this.userModel = userModel;
        }

        private void CashiersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int cashierId = ((Cashier)((ListView)sender).SelectedItem).Id;

            Model.SellectedCashierId = cashierId;
            Model.UpdateCashierStats();
        }
        private void AddCashier_Click(object sender, RoutedEventArgs e)
        {
            Model.AddCashier();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Model.UpdateCashierStats();
        }
    }
}