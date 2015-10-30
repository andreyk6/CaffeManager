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

        public ManagerMainPage(CafeManagerLib.SharedModels.UserClientModel userModel)
        {
            _context = CaffeDataContext.Instance;
            InitializeComponent();

            Model = new ManagerMainPageModel()
            {
                Name = userModel.Name,
                StatsPeriod = 0,
                Cashiers = new DataServiceCollection<Cashier>(),
                CashierStats = new List<CashierStatsModel>()
            };

            DataContext = Model;
            try
            {
                var manager = _context.Managers.Where(m => m.Name == Model.Name).FirstOrDefault();

                var cashiersQuery = from cashier in _context.Cashiers
                                    where cashier.ManagerId == manager.Id
                                    select cashier;

                Model.Cashiers = new DataServiceCollection<Cashier>(cashiersQuery);
            }
            catch (DataServiceQueryException ex)
            {
                MessageBox.Show("The query could not be completed:\n" + ex.ToString());
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show("The following error occurred:\n" + ex.ToString());
            }
        }
       

        private void CashiersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int cashierId = ((Cashier)((ListView)sender).SelectedItem).Id;

            Model.CashierStats = new List<CashierStatsModel>();
            Model.CashierStats = CaffeDataServiceExtension.GetCashierStats(_context, cashierId, Model.StatsPeriod).ToList();
        }

        private void AddCashier_Click(object sender, RoutedEventArgs e)
        {
            var manager = (from m in _context.Managers.Expand("Cashiers")
                           where m.Name == Model.Name
                           select m).Single();

            _context.LoadProperty(manager, "Cashiers");
            var newCashier = Cashier.CreateCashier(5, manager.Id);

            var result = new CashierAddWindow(newCashier);
            if (result.ShowDialog() == true)
            {
                try
                {
                    _context.AddRelatedObject(manager, "Cashiers", newCashier);
                    _context.SetLink(newCashier, "Manager", manager);

                    manager.Cashiers.Add(newCashier);
                    newCashier.Manager = manager;

                    // Send the insert to the data service.
                    DataServiceResponse response = _context.SaveChanges();

                    // Enumerate the returned responses.
                    foreach (ChangeOperationResponse change in response)
                    {
                        // Get the descriptor for the entity.
                        EntityDescriptor descriptor = change.Descriptor as EntityDescriptor;

                        if (descriptor != null)
                        {
                            Cashier addedCashier = descriptor.Entity as Cashier;

                            if (addedCashier != null)
                            {
                                Console.WriteLine("New product added with ID {0}.",
                                    addedCashier.Id);
                            }
                        }
                    }
                    MessageBox.Show("Seccess");
                }
                catch (DataServiceRequestException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Error");
            }
        }
    }
}