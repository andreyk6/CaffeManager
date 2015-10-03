using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaffeManagerServer.Model
{
    public class Manager : User
    {
        public Manager() : this("login", "password") { }
        public Manager(string login, string password)
            : base(login, password)
        { }

        public virtual ICollection<Cashier> Cashiers { get; set; }
    }
}