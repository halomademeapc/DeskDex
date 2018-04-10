using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DeskDexCore.Models
{
    public class DeskContext : DbContext
    {
        public DbSet<Checkin> Checkins { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<WorkStyle> WorkStyles { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
    }
}
