using BeautyCentersProject.Models;
using BeautyCentersProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BeautyCentersProject.Controllers
{
    public class CenterIfromationController : Controller
    {
        BeautyCenterDBEntities context = new BeautyCenterDBEntities();
        public ActionResult AboutCenter(int CenterID)
        {
            var Center = (from C in context.BeautyCenters
                         where C.ID == CenterID
                         select C).FirstOrDefault();

            var user = (from VAR in context.AspNetUserLogins select VAR);
            var employess = (from emp in context.Employees where emp.CenterID == CenterID select emp).ToList();
            CenterInformationViewModel beautyCenter = new CenterInformationViewModel()
            {
                ID = Center.ID,
                Services = Center.CentersServices.ToList(),
                Name = Center.Name,
                Address = Center.Address,
                Gender = Center.Gender,
                Description = Center.Description,
                Logo = Center.Logo,
                cityName = Center.City.City1,
                CenterWorksHours = Center.CenterWorksHours.ToList(),
                Employees = employess,
                TotalRate = Center.TotalRate,

            };

            return View(beautyCenter);
        }

        //public ActionResult AddComment(string comment, int CenterID, string ClientName)
        //{
        //    if (ModelState.IsValid == true)
        //    {
        //        var clientID = context.AspNetUsers.Where(c => c.Name == ClientName).Select(c => c.Id).FirstOrDefault();
        //        ClientCenterComment clientcomment = new ClientCenterComment();
        //        clientcomment.ClientID = clientID;
        //        clientcomment.CenterID = CenterID;
        //        clientcomment.Comment = comment;
        //        context.ClientCenterComments.Add(clientcomment);
        //        return RedirectToAction("GetBeautyCenter", CenterID);
        //    }
        //    return RedirectToAction("GetBeautyCenter", CenterID);
        //}

    }
}