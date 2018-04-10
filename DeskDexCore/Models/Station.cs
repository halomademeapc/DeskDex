﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DeskDexCore.Models
{
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
        [Range(0.0, 1)]
        public float x1 { get; set; }

        [Display(Name = "Top Edge")]
        [Range(0.0, 1)]
        public float y1 { get; set; }

        [Display(Name = "Right Edge")]
        [Range(0.0, 1)]
        public float x2 { get; set; }

        [Display(Name = "Bottom Edge")]
        [Range(0.0, 1)]
        public float y2 { get; set; }

        [Range(0, 7)]
        public int Floor { get; set; }

        [Display(Name = "Picture")]
        public string FilePath { get; set; }
    }
}