using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DeskDexCore.Models
{
    public class Floor
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        [Column(TypeName = "VARCHAR(30)")]
        [MaxLength(30)]
        public string Name { get; set; }

        [Display(Name = "Map (SVG)")]
        [StringLength(80)]
        [MaxLength(80)]
        [Column(TypeName = "VARCHAR(80)")]
        public string FilePath { get; set; }

        [Display(Name = "Sort Name")]
        [StringLength(30)]
        [MaxLength(30)]
        [Column(TypeName = "VARCHAR(30)")]
        public string SortName { get; set; }
    }
}
