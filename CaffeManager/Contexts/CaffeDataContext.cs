using CaffeManager.CafeManagerServiceReference;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaffeManager.Contexts
{
    public class CaffeDataContext
    {
        private static CaffeDbContext _instance;
        private static string _accessToken;

        public static CaffeDbContext Instance
        {
            get
            {
                if (_instance == null)
                    throw new NullReferenceException("You need to call InitializeContext before using");
                return _instance;
            }
        }
        public static void InitializeContext(string appPath, string accessToken)
        {
            CaffeDbContext context = new CaffeDbContext(new Uri(appPath));
            context.SendingRequest += new EventHandler<SendingRequestEventArgs>(OnSendingRequest);
            context.MergeOption = System.Data.Services.Client.MergeOption.OverwriteChanges;
            _instance = context;
        }

        private static void OnSendingRequest(object sender, SendingRequestEventArgs e)
        {
            // Add an Authorization header that contains an OAuth WRAP access token to the request.
            e.RequestHeaders.Add("Authorization", "Bearer " + _accessToken);
        }
    }
}
