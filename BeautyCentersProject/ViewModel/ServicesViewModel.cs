using BeautyCentersProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeautyCentersProject.ViewModel
{
    public class ServicesViewModel
    {
        public int ID { get; set; }
        public int centerID { get; set; }
        public int serviceID { get; set; }
    }
    public class servicsListViewModel
    {
        public List<Service> services { get; set; }

        public List<ServicesViewModel> ServiceList { get; set; } = new List<ServicesViewModel>();
    }
}