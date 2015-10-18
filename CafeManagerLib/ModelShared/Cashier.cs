using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CafeManagerLib.ModelShared
{
    public class Cashier : User
    {
        private Cashier() { }
        public Cashier(string login, string password)
            : base(login, password)
        {
            this.Role = UserRoles.Cashier.ToString();
        }

        public int ManagerId { get; set; }
        public virtual Manager Manager { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}