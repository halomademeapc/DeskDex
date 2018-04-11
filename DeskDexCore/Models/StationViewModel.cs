using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeskDexCore.Models
{
    public class StationViewModel
    {
        public Station Station { get; set; }
        public IEnumerable<SelectListItem> AllEquipment { get; set; }
        public IEnumerable<SelectListItem> AllWorkStyles { get; set; }

        public int selectedWorkStyle { get; set; }

        [Display(Name = "Image")]
        public IFormFile File { get; set; }

        private List<int> _selectedEquipment;
        public List<int> SelectedEquipment
        {
            get
            {
                if (_selectedEquipment == null)
                {
                    try
                    {
                        _selectedEquipment = Station.StationEquipments.Select(e => e.EquipmentId).ToList();
                    }
                    catch (Exception e)
                    {
                        //
                    }
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
