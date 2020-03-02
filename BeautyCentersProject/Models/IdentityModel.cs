using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeautyCentersProject.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        [StringLength(6)]
        public string Gender { get; set; }

    }
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext():
            base(@"Data Source = DESKTOP-97UKP06\SQL2017; Initial Catalog = BeautyCenterDB; Integrated Security = True")
        {

        }
    }
}