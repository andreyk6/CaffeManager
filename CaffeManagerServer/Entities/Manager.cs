using CaffeManagerLib.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaffeManagerServer.Enitites
{
    public class Manager : User
    {
        private Manager() { }
        public Manager(string login, string password)
            : base(login, password)
        {
            this.Role = UserRoles.Manager.ToString();
        }

        public int SuperUserId { get; set; }
        public virtual Superuser Superuser { get; set; }
        public virtual ICollection<Cashier> Cashiers { get; set; }
    }
}