﻿using System.Collections.Generic;
using System.Web.Http;

namespace WebApi.Controllers
{
    [ServiceRequestActionFilter]
    public class ValuesController : ApiController
    {
        // GET api/values 
        public IEnumerable<string> GetAll()
        {
            return new string[] { "statelssValue1", "statelessValue2" };
        }

        // GET api/values/5 
        public string Get(int id)
        {
            return "statelessValue";
        }

        // POST api/values 
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5 
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5 
        public void Delete(int id)
        {
        }
    }
}
