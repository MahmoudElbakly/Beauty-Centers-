using BeautyCentersProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeautyCentersProject.ViewModel
{
    public class EmployeeVM
    {
        public List<CentersService> ServiceCenterList { get; set; }

        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public string image { get; set; }
        public bool isDeleted { get; set; }


    }
}