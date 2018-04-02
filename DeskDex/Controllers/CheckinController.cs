using DeskDex.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Web.Http;

namespace DeskDex.Controllers
{
    public class CheckinController : ApiController
    {
        // GET: api/Checkin
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Checkin/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Checkin
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Checkin/5
        public IHttpActionResult Put(int id, [FromBody]string checkinMAC)
        {
            string CurrentUser = User.Identity.Name;
            DateTime submitTime = DateTime.Now;
            try
            {
                PhysicalAddress physicalAddress = PhysicalAddress.Parse(checkinMAC);

                // write checkin to database
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }

            return Ok();
        }

        // DELETE: api/Checkin/5
        public void Delete(int id)
        {
        }
    }
}
