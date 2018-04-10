using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeskDexCore.Models
{
    public class DeskDetailApiModel
    {
        public int DeskID { get; set; }
        public string WorkStyle { get; set; }
        public DateTime? LastUpdate { get; set; }
        public string UserName { get; set; }
        public int Capacity { get; set; }
        public List<String> Equipment { get; set; }
        public string Location { get; set; }
        public string ImagePath { get; set; }
    }
}
