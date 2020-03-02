using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautyCentersProject.ViewModel
{
    public class OrderEmployee
    {
        public int title { get; set; }
        public DateTime start1 { get; set; }
        public DateTime? end1 { get; set; }
        public int? resourceId { get; set; }
        public string client { get; set; }
    }
}