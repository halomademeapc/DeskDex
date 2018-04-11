using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DeskDexCore.Models
{
    public class Station
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Display(Name = "MAC Address")]
        [Required]
        public string PhysicalAddress { get; set; }

        [Display(Name = "Station Number")]
        [Required]
        public string Location { get; set; }

        [Display(Name = "Available Equipment")]
        public virtual ICollection<StationEquipment> StationEquipments { get; set; }
        //public virtual ICollection<Equipment> Equipment { get; set; }

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

        [Display(Name = "Floor")]
        public virtual Floor Floor { get; set; }

        [Display(Name = "Picture")]
        public string FilePath { get; set; }
    }

    public class StationEquipment
    {
        public int StationId { get; set; }
        public Station Station { get; set; }
        public int EquipmentId { get; set; }
        public Equipment Equipment { get; set; }
    }
}
