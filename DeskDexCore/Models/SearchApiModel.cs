using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeskDexCore.Models
{
    public class SearchApiModel
    {
        public IEnumerable<SearchLink> People { get; set; }
        public IEnumerable<SearchLink> Stations { get; set; }
    }

    public class SearchLink
    {
        public string Display { get; set; }
        public string Link { get; set; }
    }
}
