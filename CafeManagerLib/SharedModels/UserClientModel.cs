using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManagerLib.SharedModels
{
    public class UserClientModel
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public String Role { get; set; }
        public string Token { get; set; }
    }
}
