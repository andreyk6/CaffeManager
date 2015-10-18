using CafeManagerLib.Client;
using CafeManagerLib.ModelShared;
using CaffeManagerServer.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

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
            User user = _db.GetUserByName(User.Identity.Name);
            return new UserClientModel(user, Request.Headers.Authorization.Parameter);
        }
    }
}
