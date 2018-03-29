using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;

namespace DeskDex.Models
{
    public class Checkin
    {
        public int ID { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Last Check-in")]
        public DateTime LastUpdate { get; set; }
    }
    public class Station
    {
        public int ID { get; set; }

        [Display(Name = "MAC Address")]
        public PhysicalAddress PhysicalAddress { get; set; }

        [Display(Name = "Station Number")]
        public string Location { get; set; }

        [Display(Name = "Available Equipment")]
        public List<Equipment> Equipment { get; set; }

        public Checkin LastCheckin { get; set; }
    }

    public class Desk : Station
    {
        [Display(Name = "Type")]
        public WorkStyle Type { get; set; }
    }

    public class ConferenceRoom : Station
    {
        public int Capacity { get; set; }
    }

    public class WorkStyle
    {
        public int ID { get; set; }
        public WorkStyle(string Name)
        {
            this.Name = Name;
        }
        public string Name { get; set; }
    }

    public class Equipment
    {
        public int ID { get; set; }
        public Equipment(string Name, string Description)
        {
            this.Name = Name;
            this.Description = Description;
        }

        public string Name { get; set; }
        public string Description { get; set; }
    }
}