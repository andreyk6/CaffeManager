using CafeManagerLib.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CaffeManager
{
    public class WebApiClient
    {
        private string _appPath;
        private string _token;

        public WebApiClient(string appPath, string login, string password)
        {
            _appPath = appPath;
            _token = GetToken(appPath, login, password);
            if (_token == null)
            {
                throw new MemberAccessException(String.Format("Couldn`t login at {0}, as {1}:{2}", 
                    appPath, login, password)
                    );
            }
        }
        public UserClientModel GetUserInfo()
        {
            string getUserInfoPath = "/api/User/Info";
            try
            {
                using (var client = CreateClient(_token))
                {
                    var response = client.PostAsync(_appPath + getUserInfoPath, null).Result;
                    var result = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<UserClientModel>(result);
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private static HttpClient CreateClient(string accessToken = "")
        {
            var client = new HttpClient();
            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            }
            return client;
        }

        private static String GetToken(string appPath,string userName, string password)
        {
            var pairs = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>( "grant_type", "password" ),
                    new KeyValuePair<string, string>( "username", userName ),
                    new KeyValuePair<string, string> ( "Password", password )
                };
            var content = new FormUrlEncodedContent(pairs);

            using (var client = new HttpClient())
            {
                var response =
                    client.PostAsync(appPath + "/Token", content).Result;
                var result = response.Content.ReadAsStringAsync().Result;
                // Десериализация полученного JSON-объекта
                Dictionary<string, string> tokenDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(result);

                if (tokenDictionary.ContainsKey("access_token"))
                {
                    return tokenDictionary["access_token"];
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
