using CafeManagerLib.ModelShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManagerLib.Client
{
    public class UserClientModel
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public String Role { get; set; }
        public string Token { get; set; }
        public UserClientModel() { }
        public UserClientModel(User user, string token)
        {
            this.Login = user.Login;
            this.Name = user.Name;
            this.Role = user.Role;
            this.Token = token;
        }
    }
}
