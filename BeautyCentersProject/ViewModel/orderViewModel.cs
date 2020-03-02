using BeautyCentersProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautyCentersProject.ViewModel
{
    public class orderViewModel
    {
        public int ClientID { get; set; }
        public int OrderID { get; set; }
        public List<Order> Orders { get; set; }
        public List<OrderServiceEmployee> ServiceEmployees { get; set; }
        public List<Service> Services { get; set; }
        public List<Offer> Offers { get; set; }
    }
}