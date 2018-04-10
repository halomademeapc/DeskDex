using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
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
        [Range(0.0,1)]
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

        [Range(0,7)]
        public int Floor { get; set; }

        [Display(Name ="Picture")]
        public string FilePath { get; set; }
    }

    public class WorkStyle
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public Int32 Argb
        {
            get
            {
                return Color.ToArgb();
            }
            set
            {
                Color = Color.FromArgb(value);
            }
        }

        [NotMapped]
        public Color Color { get; set; }

        [NotMapped]
        [Display(Name = "Color on Map")]
        public string HexColor
        {
            get
            {
                return "#" + Color.R.ToString("X2") + Color.G.ToString("X2") + Color.B.ToString("X2");
            }
            set
            {
                int argb = Int32.Parse(value.Replace("#", ""), System.Globalization.NumberStyles.HexNumber);
                Color = Color.FromArgb(argb);
            }
        }
    }

    public class Equipment
    {
        public Equipment()
        {
            this.Stations = new HashSet<Station>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Station> Stations { get; set; }

    }
}