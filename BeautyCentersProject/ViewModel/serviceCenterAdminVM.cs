using BeautyCentersProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautyCentersProject.ViewModel
{
    public class serviceCenterAdminVM
    {
        public int serviceID { get; set; }
        public List<Service> services { get; set; }
        public List<CentersService> centerServiceList { get; set; }

    }
}