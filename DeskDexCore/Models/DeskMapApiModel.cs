using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeskDexCore.Models
{
    public class DeskMapApiModel
    {
        public int DeskID { get; set; }
        public float x1 { get; set; }
        public float x2 { get; set; }
        public float y1 { get; set; }
        public float y2 { get; set; }
        public string WorkStyle { get; set; }
        public DateTime? LastCheckin { get; set; }
        public string Location { get; set; }
    }
}
