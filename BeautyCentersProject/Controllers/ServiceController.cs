using BeautyCentersProject.Models;
using BeautyCentersProject.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BeautyCentersProject.Controllers
{
    public class ServiceController : Controller
    {
        BeautyCenterDBEntities context = new BeautyCenterDBEntities();
        // GET: Service
        public ActionResult getServices()
        {
            List<Service> services = context.Services.Where(ser => ser.IsDeleted == false).ToList();
            return View(services);
        }
        public ActionResult Create()
        {
            ServiceViewModel service = new ServiceViewModel();
            return View(service);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(ServiceViewModel newService)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // save image
                    string ImageFileName;
                    string fileName = Path.GetFileNameWithoutExtension(newService.file.FileName);
                    string extension = Path.GetExtension(newService.file.FileName);
                    fileName = fileName + DateTime.Now.Millisecond.ToString() + extension;
                    ImageFileName = fileName;
                    newService.Image = "~/Content/Images/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Content/Images/"), fileName);
                    newService.file.SaveAs(fileName);
                    // save service
                    Service service = new Service();
                    service.Name = newService.Name;
                    service.Image = ImageFileName;
                    context.Services.Add(service);
                    context.SaveChanges();
                    return RedirectToAction("getServices");
                }

                return View(newService);
            }
            catch (Exception)
            {
                return View(newService);
            }
        }

        public ActionResult Edit(int id)
        {
            Service service = context.Services.Where(s => s.ID == id).FirstOrDefault();
            ServiceViewModel serviceVM = new ServiceViewModel()
            {
                Name = service.Name,
                Image = service.Image
            };
            return View(serviceVM);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(ServiceViewModel newService)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Service oldService = context.Services.Where(s => s.ID == newService.ID).FirstOrDefault();
                    if (oldService.Name != newService.Name)
                    {
                        bool isFound = context.Services.Any(s => s.Name == newService.Name && s.IsDeleted == false);
                        if (isFound)
                        {
                            ModelState.AddModelError("", "اسم الخدمة موجود بالفعل");
                            return View(newService);
                        }
                    }
                    // save image
                    string ImageFileName;
                    string fileName = Path.GetFileNameWithoutExtension(newService.file.FileName);
                    string extension = Path.GetExtension(newService.file.FileName);
                    fileName = fileName + DateTime.Now.Millisecond.ToString() + extension;
                    ImageFileName = fileName;
                    newService.Image = "~/Content/Images/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Content/Images/"), fileName);
                    newService.file.SaveAs(fileName);
                    // save service
                    oldService.Name = newService.Name;
                    oldService.Image = ImageFileName;
                    context.Entry(oldService).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    return RedirectToAction("getServices");
                }
                return View(newService);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "لقد حدث خطأ في البيانات");
                return View(newService);
            }
        }

        public ActionResult Delete(int id)
        {
            Service oldService = context.Services.Where(s => s.ID == id).FirstOrDefault();
            oldService.IsDeleted = true;
            context.Entry(oldService).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
            return RedirectToAction("getServices");
        }
    }
}