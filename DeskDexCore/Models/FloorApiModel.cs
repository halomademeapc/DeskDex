using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeskDexCore.Models
{
    public class FloorApiModel
    {
        public List<DeskMapApiModel> Stations { get; set; }
        public string FloorName { get; set; }
        public string FloorImage { get; set; }
        public DateTime Generated { get; set; }
    }
}
