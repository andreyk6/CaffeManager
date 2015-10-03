using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaffeManagerServer.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Login { get; private set; }

        public User() : this("baseLogin", "basePassword") { }

        public User(string login, string password)
        {
            Login = login;
            _password = password; 
        }

        public bool Loggin(string password)
        {
            return password == _password;
        }

        private string _password;
    }
}