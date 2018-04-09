using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public int selectedWorkStyle { get; set; }

        [Display(Name = "Image")]
        public HttpPostedFileBase File { get; set; }

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

    public class DeskMapViewModel
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

    public class DeskDetailViewModel
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

    public class CheckinViewModel
    {
        public string address { get; set; }
        public string acid { get; set; }
    }
}