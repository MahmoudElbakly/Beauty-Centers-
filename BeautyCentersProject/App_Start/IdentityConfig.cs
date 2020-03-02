using BeautyCentersProject.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautyCentersProject
{
    public class ApplicationStore: UserStore<ApplicationUser>
    {
        public ApplicationStore(ApplicationDbContext context):base(context)
        {

        }
    }

    public class ApplicationManager: UserManager<ApplicationUser>
    {
        public ApplicationManager(ApplicationStore store): base(store)
        {

        }
    }
}