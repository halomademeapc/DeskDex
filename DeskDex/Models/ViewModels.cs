using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeskDex.Models
{
    public class StationViewModel
    {
        public Station Station { get; set; }
        public IEnumerable<SelectListItem> AllEquipment { get; set; }
        public IEnumerable<SelectListItem> AllWorkStyles { get; set; }

        private List<int> _selectedEquipment;
        public List<int> SelectedEquipment
        {
            get
            {
                if (_selectedEquipment == null)
                {
                    _selectedEquipment = Station.Equipment.Select(e => e.ID).ToList();
                }
                return _selectedEquipment;
            }
            set
            {
                _selectedEquipment = value;
            }
        }
    }
}