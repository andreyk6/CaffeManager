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

namespace CaffeManager.View
{
    /// <summary>
    /// Логика взаимодействия для ManagerMainPage.xaml
    /// </summary>
    public partial class ManagerMainPage : Page
    {
        public ManagerMainPageModel Model { get; set; }
        CaffeDbContext _context;
        WebApiClient _client;

        public ManagerMainPage(WebApiClient client)
        {
            InitializeComponent();

            _client = client;

            Model = new ManagerMainPageModel()
            {
                Name = client.GetUserInfo().Name,
                Cashiers = new DataServiceCollection<Cashier>(),
                CashierOrders = new DataServiceCollection<Order>()
            };

            _context = InitContext();

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
        private CaffeDbContext InitContext()
        {
            CaffeDbContext context = new CaffeDbContext(new Uri(_client.AppPath + "/CaffeDataService.svc"));
            context.SendingRequest += new EventHandler<SendingRequestEventArgs>(OnSendingRequest);
            return context;
        }
        private void OnSendingRequest(object sender, SendingRequestEventArgs e)
        {
            // Add an Authorization header that contains an OAuth WRAP access token to the request.
            e.RequestHeaders.Add("Authorization", "Bearer " + _client.Token);
        }

        private void CashiersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int cashierId = ((Cashier)((ListView)sender).SelectedItem).Id;

            var orders = _context.Orders.Where(oi => oi.CashierId == cashierId);

            Model.CashierOrders = new DataServiceCollection<Order>(orders);
        }
    }
}