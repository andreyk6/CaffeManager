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
        // GET: api/User
        [HttpGet]
        public IEnumerable<string> A()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/User/5
        [Authorize]
        [HttpGet]
        public string B(int id)
        {
            return "value";
        }

        [HttpGet]
        [Authorize(Roles ="Manager")]
        public int D(int id,int id2)
        {
            return id;
        }

        // POST: api/User
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/User/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/User/5
        public void Delete(int id)
        {
        }
    }
}
