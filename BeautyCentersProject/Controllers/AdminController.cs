using BeautyCentersProject.Models;
using BeautyCentersProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BeautyCentersProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        BeautyCenterDBEntities context = new BeautyCenterDBEntities();
        // GET: Admin
        public ActionResult getCenters()
        {
            beautyCenterViewModel BeautyCenter = new beautyCenterViewModel();
            BeautyCenter.centerList = context.BeautyCenters.Where(c => c.IsDeleted == false).ToList();
            return View(BeautyCenter);
        }

        public ActionResult filterCenter(int centerFilter)
        {
            List<BeautyCenter> centers = new List<BeautyCenter>();
            beautyCenterViewModel centerList = new beautyCenterViewModel();
            if (centerFilter == 0)
            {
                centers = context.BeautyCenters.Where(c => c.IsDeleted == false).ToList();
            }
            else if (centerFilter == 1)
            {
                centers = context.BeautyCenters.Where(c => c.IsDeleted == false && c.IsApproved == true).ToList();
            }
            else
            {
                centers = context.BeautyCenters.Where(c => c.IsDeleted == false && c.IsApproved == false).ToList();
            }
            centerList.centerList = centers;
            return PartialView("_IsApproviedCenters", centerList);
        }

        [HttpPost]
        public ActionResult centerApprovied(string approvied, string centerID)
        {
            int cID = int.Parse(centerID);
            var center = context.BeautyCenters.Where(c => c.ID == cID).FirstOrDefault();

            if (approvied == "1")
            {
                center.IsApproved = true;
            }
            else if (approvied == "2")
            {
                center.IsApproved = false;
            }
            context.Entry(center).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
            return RedirectToAction("getCenters", "Admin");
        }

        public ActionResult Details(int id)
        {
            centerDetailsViewModel center = new centerDetailsViewModel();

            center.Center = context.BeautyCenters.Where(c => c.ID == id).FirstOrDefault();
            center.centerServices = context.CentersServices.Where(c => c.CenterID == id).ToList();
            center.Employees = context.Employees.Where(c => c.CenterID == id).ToList();
            return View(center);
        }

        public ActionResult centerInfo(int centerFilter, int centerID)
        {
            centerDetailsViewModel centerList = new centerDetailsViewModel();
            if (centerFilter == 0)
            {
                centerList.Center = context.BeautyCenters.Where(c => c.ID == centerID).FirstOrDefault();
                return PartialView("_CenterData", centerList);
            }
            else if (centerFilter == 1)
            {
                centerList.centerServices = context.CentersServices.Where(c => c.isDeleted == false && c.ID == centerID).ToList();
                return PartialView("_CenterserviceData", centerList);
            }
            else
            {
                centerList.Employees = context.Employees.Where(c => c.IsDeleted == false && c.CenterID == centerID).ToList();
                return PartialView("_CenterEmployeeData", centerList);
            }
        }
    }
}