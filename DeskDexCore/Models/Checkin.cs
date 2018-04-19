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
        public int ID { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Last Check-in")]
        public DateTime LastUpdate { get; set; }

        public string Display { get; set; }
    }
}
