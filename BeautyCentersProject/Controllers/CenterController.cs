using BeautyCentersProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BeautyCentersProject.Controllers
{
    public class CenterController : Controller
    {
        BeautyCenterDBEntities context = new BeautyCenterDBEntities();
        // GET: Center
        public ActionResult getCenters()
        {
            List<BeautyCenter> centers = context.BeautyCenters.Where(c => c.IsDeleted == false).ToList();
            return View(centers);
        }
    }
}