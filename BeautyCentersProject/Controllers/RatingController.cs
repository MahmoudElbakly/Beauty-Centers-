using BeautyCentersProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BeautyCentersProject.Controllers
{
    [Authorize(Roles ="Client")]
    public class RatingController : Controller
    {
        BeautyCenterDBEntities context = new BeautyCenterDBEntities();
        // GET: Rating
      
        public ActionResult RateService(int? ratevalue ,int? CenterID, int? serviceID)
        {
            var centerServicesID = (from cs in context.CentersServices
                                    where cs.CenterID == CenterID && cs.ServiceID == serviceID && cs.isDeleted == false
                                    select cs.ID).FirstOrDefault();

            var username = User.Identity.Name;
            ApplicationManager manager = new ApplicationManager(new ApplicationStore(new ApplicationDbContext()));
            ApplicationUser user = manager.FindByNameAsync(username).Result;
            if (ratevalue != null && CenterID != null && serviceID != null)
            {

           
            var IFHasOLDRAte = (from CR in context.ClientRates
                                where CR.ClientID == user.Id && CR.CenterServiceID == centerServicesID
                                select CR).FirstOrDefault();
            if (IFHasOLDRAte != null)
            {
                int rate = ratevalue ?? default(int);
                IFHasOLDRAte.rate = rate;
                context.SaveChanges();
            }
            else {


                ClientRate clientrate = new ClientRate();
                clientrate.ClientID = user.Id;
                clientrate.CenterServiceID = centerServicesID;
                int rate = ratevalue ?? default(int);
                clientrate.rate = rate;

                context.ClientRates.Add(clientrate);
                context.SaveChanges();
            }
            }
            ClientRate Rate = new ClientRate();
            if( ratevalue == null && CenterID == null && serviceID == null){

                ClientRate NewRate = new ClientRate();
                Rate = NewRate;

            }
            else{ 
            var NewRate = (from CR in context.ClientRates
                                where CR.ClientID == user.Id && CR.CenterServiceID == centerServicesID
                                select CR).FirstOrDefault();
                Rate = NewRate;
            }
            return PartialView("_RateService", Rate);
        }
    }
}