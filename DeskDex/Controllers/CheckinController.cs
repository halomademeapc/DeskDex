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
        public IEnumerable<Checkin> Get()
        {
            List<Checkin> Checkins = new List<Checkin>();
            using (var db = new DeskContext())
            {
                var query = from c in db.Checkins orderby c.Username select c;
                foreach (var item in query)
                {
                    Checkins.Add((Checkin)item);
                }
            }
            return Checkins;
        }

        // GET: api/Checkin/5
        public Checkin Get(int id)
        {
            Checkin checkin = new Checkin();
            using (var db = new DeskContext())
            {
                var query = from c in db.Checkins where c.ID == id select c;
                foreach (var item in query)
                {
                    checkin = (Checkin)item;
                }
            }
            return checkin;
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
                using (var db = new DeskContext())
                {
                    var checkin = new Checkin
                    {
                        LastUpdate = submitTime,
                        Username = CurrentUser
                    };

                    db.Checkins.Add(checkin);
                    db.SaveChanges();
                }
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
