using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautyCentersProject.Controllers
{
    public class TestOrderVM
    {
        public int ID { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }

        public int statusID { get; set; }
    }
}