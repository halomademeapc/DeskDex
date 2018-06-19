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
        [Column("StationID")]
        public int ID { get; set; }

        [Display(Name = "MAC Address")]
        [Required]
        [MaxLength(30)]
        [StringLength(30)]
        [Column(TypeName = "VARCHAR(30)")]
        public string PhysicalAddress { get; set; }

        [Display(Name = "Station Number")]
        [Required]
        [StringLength(20)]
        [MaxLength(20)]
        [Column(TypeName = "VARCHAR(20)")]
        public string Location { get; set; }

        [Display(Name = "Available Equipment")]
        public virtual ICollection<StationEquipment> StationEquipments { get; set; }

        [Column("CheckinID")]
        public virtual Checkin LastCheckin { get; set; }

        public int Capacity { get; set; }

        [Display(Name = "Type")]
        [Column("WorkStyleID")]
        public virtual WorkStyle Type { get; set; }

        [Column("Left")]
        [Display(Name = "Left Edge")]
        [Range(0.0, 1)]
        public float x1 { get; set; }

        [Column("Top")]
        [Display(Name = "Top Edge")]
        [Range(0.0, 1)]
        public float y1 { get; set; }

        [Column("Right")]
        [Display(Name = "Right Edge")]
        [Range(0.0, 1)]
        public float x2 { get; set; }

        [Column("Bottom")]
        [Display(Name = "Bottom Edge")]
        [Range(0.0, 1)]
        public float y2 { get; set; }

        [Display(Name = "Rotation")]
        [Range(0, 360)]
        public int Rotation { get; set; }

        [Display(Name = "Floor")]
        [Column("FloorID")]
        public virtual Floor Floor { get; set; }

        [Display(Name = "Picture")]
        [StringLength(80)]
        [MaxLength(80)]
        [Column(TypeName = "VARCHAR(80)")]
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
