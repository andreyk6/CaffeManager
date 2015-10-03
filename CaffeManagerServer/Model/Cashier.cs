using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaffeManagerServer.Model
{
    public class Cashier : User
    {
        public Cashier() : this("login", "password") { }
        public Cashier(string login, string password)
            : base(login, password)
        { }

        public int ManagerId { get; set; }
        public virtual Manager Manager { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}