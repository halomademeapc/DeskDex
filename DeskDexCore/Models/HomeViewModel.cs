using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeskDexCore.Models
{
    public class HomeViewModel
    {
        public string MainImage {get
            {
                return _homeImages[new Random().Next(_homeImages.Count)];
            }
        }
        public int UserCount { get; set; }
        public int StationCount { get; set; }
        public int OpenRatio { get; set; }

        private static List<string> _homeImages = new List<string>
        {
            "/images/homebg.jpg",
            "/images/homebg1.jpg",
            "/images/homebg2.jpg",
            "/images/homebg3.jpg"
        };
    }
}
