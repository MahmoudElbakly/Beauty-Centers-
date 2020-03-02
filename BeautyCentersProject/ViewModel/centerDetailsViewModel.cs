using BeautyCentersProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautyCentersProject.ViewModel
{
    public class centerDetailsViewModel
    {
        public BeautyCenter Center { get; set; }
        public List<CentersService> centerServices { get; set; }
        public List<Employee> Employees { get; set; }

    }
}