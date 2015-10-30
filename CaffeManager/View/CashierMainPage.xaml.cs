using CafeManagerLib.SharedModels;
using CaffeManager.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace CaffeManager.View
{
    /// <summary>
    /// Логика взаимодействия для CashierMainPage.xaml
    /// </summary>
    public partial class CashierMainPage : Page
    {
        private CashierPageModel _model;
        public CashierMainPage(UserClientModel userModel)
        {
            InitializeComponent();
            _model = new CashierPageModel()
            {
                Name = userModel.Name
            };
            DataContext = _model;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //decimal count;
            //try {
            //    count = decimal.Parse(tb_count.Text);
            //        }
            //catch(Exception ex)
            //{
            //    count = 1;
            //}
            //_model.AddNewItemToCurrentOrder(((ListView)sender).SelectedIndex, count);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            decimal count;
            try
            {
                count = decimal.Parse(tb_count.Text);
            }
            catch (Exception ex)
            {
                count = 1;
            }
            _model.AddNewItemToCurrentOrder(menuList.SelectedIndex, count);
            ICollectionView view = CollectionViewSource.GetDefaultView(currOrd.ItemsSource);
            view.Refresh();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _model.Save();
        }
    }
}
