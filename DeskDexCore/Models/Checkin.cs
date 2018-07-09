using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DeskDexCore.Models
{
    public class Checkin
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("CheckinID")]
        public int ID { get; set; }

        [Required]
        [StringLength(25)]
        [MaxLength(25)]
        [Column(TypeName = "VARCHAR(25)")]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Last Check-in")]
        public DateTime LastUpdate { get; set; }

        [StringLength(50)]
        [MaxLength(50)]
        [Column(TypeName = "VARCHAR(50)")]
        public string Display { get; set; }
    }
}
