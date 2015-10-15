using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaffeManagerServer.Model
{
    public class Manager : User
    {
        private Manager() { }
        public Manager(string login, string password)
            : base(login, password)
        {
            this.Role = "Manager";
        }

        public virtual ICollection<Cashier> Cashiers { get; set; }
    }
}