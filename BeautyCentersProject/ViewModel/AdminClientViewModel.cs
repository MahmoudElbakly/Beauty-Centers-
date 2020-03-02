using BeautyCentersProject.Models;
using BeautyCentersProject.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BeautyCentersProject.ViewModel
{
    public class AdminClientViewModel
    {
        public int ID { get; set; }
        [Required]
        public string CenterName { get; set; }
        public int CityID { get; set; }
        public string Logo { get; set; }
        public HttpPostedFileBase file { get; set; }
        public bool IsApproved { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public Nullable<double> TotalRate { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Address { get; set; }
        public int Gender { get; set; }

        public List<ServiceType> typeList { get; set; }

        public List<City> Cities { get; set; }

        [Required]
        public string AdminName { get; set; }
        [Required, RegularExpression(@"^[A-Za-z0-9]+(?:[_-][A-Za-z0-9]+)*$",
            ErrorMessage = "Enter valid username, username should contains to characters, digits and (-_)")]
        public string Username { get; set; }
        [Required, RegularExpression(@"^[a-zA-Z]+(\.)?[a-zA-Z0-9]+@[a-z]{3,10}\.[a-z]{2,3}(\.[a-z]{2,3})?$",
            ErrorMessage = "Enter valid email like xxx.xxx@xxxx.xxx")]
        public string Email { get; set; }
        [Required, RegularExpression(@"^01(0|1|2|5)[0-9]{8}$",
            ErrorMessage = "Phone must be like 010xxxxxxxx or  011xxxxxxxx or or  012xxxxxxxx or 015xxxxxxxx")]
        public string Phone { get; set; }
        [DataType(DataType.Password), Required]
        public string Password { get; set; }
        [DataType(DataType.Password), System.ComponentModel.DataAnnotations.Compare("Password")]
        public string ConfirmPass { get; set; }

        public string UserID { get; set; }
        [Required, RegularExpression(@"^\d{14}$", ErrorMessage = "الرقم القومي يجب ان يكون 14 رقم فقط لاغير")]
        public string NID { get; set; }
        public string CreditCard { get; set; }
        public string NiDImage { get; set; }
        public int CenterID { get; set; }

        public List<Service> services { get; set; }
        public List<ServicesViewModel> ServiceList { get; set; } = new List<ServicesViewModel>();
    }
}