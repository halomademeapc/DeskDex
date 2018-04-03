using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;

namespace DeskDex.Models
{
    public class Checkin
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Last Check-in")]
        public DateTime LastUpdate { get; set; }
    }
    public class Station
    {
        public Station()
        {
            this.Equipment = new HashSet<Equipment>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Display(Name = "MAC Address")]
        public string PhysicalAddress { get; set; }

        [Display(Name = "Station Number")]
        public string Location { get; set; }

        [Display(Name = "Available Equipment")]
        public virtual ICollection<Equipment> Equipment { get; set; }

        public virtual Checkin LastCheckin { get; set; }

        public int Capacity { get; set; }

        [Display(Name = "Type")]
        public virtual WorkStyle Type { get; set; }

        [Display(Name = "Left Edge")]
        public float x1 { get; set; }

        [Display(Name = "Top Edge")]
        public float y1 { get; set; }

        [Display(Name = "Right Edge")]
        public float x2 { get; set; }
        [Display(Name = "Bottom Edge")]
        public float y2 { get; set; }
        public int Floor { get; set; }
    }

    public class WorkStyle
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class Equipment
    {
        public Equipment()
        {
            this.Stations = new HashSet<Station>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Station> Stations { get; set; }

    }
}