using CaffeManagerLib.SharedModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CaffeManagerLib.SharedModels
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public String Role { get; set; }

        protected User() { }

        public User(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
}