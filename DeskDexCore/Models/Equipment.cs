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
        /*public Equipment()
        {
            this.Stations = new HashSet<Station>();
        }*/

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        //public virtual ICollection<Station> Stations { get; set; }
        public virtual ICollection<StationEquipment> StationEquipments { get; set; }
    }
}
