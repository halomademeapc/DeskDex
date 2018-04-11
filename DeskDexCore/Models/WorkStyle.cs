using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace DeskDexCore.Models
{
    public class WorkStyle
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public Int32 Argb
        {
            get
            {
                return Color.ToArgb();
            }
            set
            {
                Color = Color.FromArgb(value);
            }
        }

        [NotMapped]
        public Color Color { get; set; }

        [NotMapped]
        [Display(Name = "Color on Map")]
        public string HexColor
        {
            get
            {
                return "#" + Color.R.ToString("X2") + Color.G.ToString("X2") + Color.B.ToString("X2");
            }
            set
            {
                int argb = Int32.Parse(value.Replace("#", ""), System.Globalization.NumberStyles.HexNumber);
                Color = Color.FromArgb(argb);
            }
        }
    }
}
