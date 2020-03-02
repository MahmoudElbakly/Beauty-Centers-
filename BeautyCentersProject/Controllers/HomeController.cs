using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BeautyCentersProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //IAuthenticationManager authentication = HttpContext.GetOwinContext().Authentication;
            return View();
        }
       
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult policy()
        {
            return View();
        }
    }
}