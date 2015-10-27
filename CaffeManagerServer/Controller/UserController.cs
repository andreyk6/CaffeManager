using CaffeManagerServer.Enitites;
using CaffeManagerServer.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CafeManagerLib.SharedModels;

namespace CaffeManagerServer.Controller
{
    public class UserController : ApiController
    {
        private CaffeDbContext _db = new CaffeDbContext();

        // GET: api/User
        [Authorize]
        [HttpPost]
        public UserClientModel Info()
        {
            try {
                User user = _db.GetUserByName(User.Identity.Name);
                return new UserClientModel()
                {
                    Login = user.Login,
                    Name = user.Name,
                    Role = user.Role,
                    Token = Request.Headers.Authorization.Parameter
                };
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
