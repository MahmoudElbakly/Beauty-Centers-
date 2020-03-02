using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeautyCentersProject.ViewModel
{
    public class AdminRegisterationViewModel
    {
        public string ID { get; set; }
        [Required]
        public string Name { get; set; }
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
        [DataType(DataType.Password), Compare("Password")]
        public string ConfirmPass { get; set; }

        public Gender Gender { get; set; }
    }
}