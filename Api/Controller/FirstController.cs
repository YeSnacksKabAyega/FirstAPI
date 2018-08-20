using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Api.Controller
{
    public class FirstController : ApiController
    {
        // GET: api/First
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/First/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/First
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/First/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/First/5
        public void Delete(int id)
        {
        }
    }
}
