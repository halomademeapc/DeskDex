using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeskDexCore.Models
{
    public class FloorViewModel
    {
        public Floor Floor { get; set; }

        [Display(Name = "Map (SVG)")]
        public IFormFile File { get; set; }
    }
}
