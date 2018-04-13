using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeskDexCore.Models
{
    public class LegendViewModel
    {
        public IEnumerable<SelectListItem> AllFloors { get; set; }
        public List<WorkStyle> AllWorkStyles { get; set; }
    }
}
