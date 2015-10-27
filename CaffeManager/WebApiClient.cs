using CafeManagerLib.SharedModels;
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
        public string AppPath { get; set; }
        public string Token { get; set; }

        public WebApiClient(string appPath, string login, string password)
        {
            AppPath = appPath;
            Token = GetToken(appPath, login, password);
            if (Token == null)
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
                using (var client = CreateClient(Token))
                {
                    var response = client.PostAsync(AppPath + getUserInfoPath, null).Result;
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
