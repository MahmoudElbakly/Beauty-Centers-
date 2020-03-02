using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeautyCentersProject.ViewModel
{
    public class ServiceViewModel
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public string Image { get; set; }

        public HttpPostedFileBase file { get; set; }
    }
}