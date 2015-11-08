using CaffeManagerLib.SharedModels;
using CaffeManagerServer.Enitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaffeManagerServer.Entities
{
    public class Superuser:User
    {
        private Superuser() { }
        public Superuser(string login, string password)
            : base(login, password)
        {
            this.Role = UserRoles.SuperUser.ToString();
        }

        public virtual ICollection<Manager> Managers { get; set; }
    }
}
