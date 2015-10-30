using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaffeManagerLib.SharedModels
{
    public class Manager : User
    {
        private Manager() { }
        public Manager(string login, string password)
            : base(login, password)
        {
            this.Role = UserRoles.Manager.ToString();
        }

        public virtual ICollection<Cashier> Cashiers { get; set; }
    }
}