using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DeskDex.Models
{
    public class DeskContext : DbContext
    {
        public DeskContext() : base(System.Configuration.ConfigurationManager.ConnectionStrings["TempDB"].ConnectionString)
        {
        }
        public DbSet<Checkin> Checkins { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<WorkStyle> WorkStyles { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
    }
}