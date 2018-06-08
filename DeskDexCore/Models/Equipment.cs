using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DeskDexCore.Models
{
    public class Equipment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        [MaxLength(30)]
        [Column(TypeName = "VARCHAR(30)")]
        public string Name { get; set; }

        [StringLength(200)]
        [MaxLength(200)]
        [Column(TypeName = "VARCHAR(200)")]
        public string Description { get; set; }

        public virtual ICollection<StationEquipment> StationEquipments { get; set; }
    }
}
