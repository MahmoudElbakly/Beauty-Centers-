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
    [Authorize(Roles = "CenterAdmin")]
    public class CenterAdminController : Controller
    {
        BeautyCenterDBEntities context = new BeautyCenterDBEntities();
        // GET: CenterAdmin
        public ActionResult Index()
        {
            try
            {
                ViewBag.MSG = TempData["Message"].ToString();
                TempData.Keep();
                return View();
            }
            catch (Exception)
            {

                return View();
            }
        }

        public ActionResult Manage()
        {
            centerDetailsViewModel centerVM = new centerDetailsViewModel();
            if (Session["Center"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            int centerID = (int)Session["Center"];
            BeautyCenter beautyCenter = context.BeautyCenters.Where(c => c.ID == centerID).FirstOrDefault();
            if (beautyCenter.IsDeleted == true)
            {
                TempData["Message"] = "نأسف لعدم قبول المركز \n فهناك بعض البيانات التي يجب إدخالها صحيح برجاء إعادة محاولة التسجيل";
                return RedirectToAction("Index");
            }

            if (beautyCenter.IsApproved == false)
            {
                TempData["Message"] = "شكرا لتسجيلكم معنا \n سوف يتم مراجعة البيانات والرد عليكم خلال 24 ساعة";
                return RedirectToAction("Index");
            }
            else
            {
                centerVM.Center = beautyCenter;
                centerVM.centerServices = context.CentersServices.Where(c => c.isDeleted == false && c.ID == centerID).ToList();
                centerVM.Employees = context.Employees.Where(c => c.IsDeleted == false && c.CenterID == centerID).ToList();
                return View(centerVM);
            }
        }

        public ActionResult getServices()
        {
            BeautyCenter center = getCenter();
            int centerID = center.ID;
            var services = context.CentersServices.Where(cs => cs.CenterID == centerID && cs.isDeleted == false).ToList();
            return View(services);
        }

        private BeautyCenter getCenter()
        {
            string username = User.Identity.Name;
            ApplicationManager manager = new ApplicationManager(new ApplicationStore(new ApplicationDbContext()));
            ApplicationUser user = manager.FindByNameAsync(username).Result;
            var center = context.CenterAdmins.Where(a => a.UserID == user.Id).Select(c => c.BeautyCenter).FirstOrDefault();
            return center;
        }

        [HttpPost]
        public void Upload()
        {
            for (int i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];

                var fileName = Path.GetFileName(file.FileName);

                var path = Path.Combine(Server.MapPath("~/Content/Images/"), fileName);
                file.SaveAs(path);
            }

        }

        [HttpPost]
        public ActionResult createTable(List<List<string>> arrayList)
        {
            if (arrayList.Count <= 0)
            {
                return Json(new { success = false, data = "invalid" });

            }
            else
            {
                Session.Clear();
                Session["ServiceCenter"] = arrayList;
                var servicesList = Session["ServiceCenter"] as List<List<string>>;
                var center = getCenter();
                addServicesToCenter(servicesList, center.ID);
                return Json(new { success = true, data = "ok" });
            }
        }

        private bool addServicesToCenter(List<List<string>> servicesList, int CenterID)
        {
            if (servicesList.Count <= 0)
                return false;
            else
            {
                foreach (var service in servicesList)
                {
                    int serviceID = int.Parse(service[0]);
                    string Description = service[2];
                    int price = int.Parse(service[3]);
                    double totalMinuts = int.Parse(service[4]);
                    var time = TimeSpan.FromMinutes(totalMinuts);
                    string[] paths = service[5].Split('\\');
                    string image = paths[2];
                    CentersService centerService = new CentersService()
                    {
                        CenterID = CenterID,
                        ServiceID = serviceID,
                        Description = Description,
                        Price = price,
                        EstimatedTime = time,
                        Image = image,
                    };
                    context.CentersServices.Add(centerService);
                    context.SaveChanges();
                }
                return false;
            }
        }

        public ActionResult createService()
        {
            BeautyCenter center = getCenter();
            serviceCenterAdminVM serviceVM = new serviceCenterAdminVM();
            serviceVM.services = context.Services.Where(c => c.IsDeleted == false).ToList();
            serviceVM.centerServiceList = context.CentersServices.Where(c => c.CenterID == center.ID && c.isDeleted == false).ToList();
            return View(serviceVM);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult createService(List<List<string>> services)
        {
            serviceCenterAdminVM serviceVM = new serviceCenterAdminVM();
            serviceVM.services = context.Services.Where(c => c.IsDeleted == false).ToList();
            return View(serviceVM);
        }

        public ActionResult EditService(int id)
        {
            var service = context.CentersServices.Where(c => c.ID == id).FirstOrDefault();
            return View(service);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult EditService(CentersService centerService)
        {
            if (!ModelState.IsValid || centerService.TimeMinutes <= 0 || centerService.Price <= 0)
            {
                ModelState.AddModelError("", "بيانات غير صحيحة");
                var service = context.CentersServices.Where(c => c.ID == centerService.ID).FirstOrDefault();
                return View(service);
            }
            CentersService oldCenterService = context.CentersServices.Where(c => c.ID == centerService.ID).FirstOrDefault();
            ;
            // save image
            if (centerService.file != null)
            {
                string ImageFileName;
                string fileName = Path.GetFileNameWithoutExtension(centerService.file.FileName);
                string extension = Path.GetExtension(centerService.file.FileName);
                fileName = fileName + DateTime.Now.Millisecond.ToString() + extension;
                ImageFileName = fileName;
                centerService.Image = "~/Content/Images/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Content/Images/"), fileName);
                centerService.file.SaveAs(fileName);

                oldCenterService.Price = centerService.Price;
                oldCenterService.EstimatedTime = TimeSpan.FromMinutes(centerService.TimeMinutes);
                oldCenterService.Description = centerService.Description;
                oldCenterService.Image = ImageFileName;

            }
            oldCenterService.Price = centerService.Price;
            oldCenterService.EstimatedTime = TimeSpan.FromMinutes(centerService.TimeMinutes);
            oldCenterService.Description = centerService.Description;

            context.Entry(oldCenterService).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
            return RedirectToAction("getServices");
        }

        public ActionResult DeleteService(int id)
        {
            CentersService oldService = context.CentersServices.Where(s => s.ID == id).FirstOrDefault();
            oldService.isDeleted = true;
            context.Entry(oldService).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
            return RedirectToAction("getServices");
        }

        public ActionResult getEmployess()
        {
            var center = getCenter();
            var employees = context.Employees.Where(e => e.CenterID == center.ID && e.IsDeleted == false).ToList();
            return View(employees);
        }

        public ActionResult createEmployee()
        {
            BeautyCenter center = getCenter();
            int centerID = center.ID;
            var services = context.CentersServices.Where(cs => cs.CenterID == centerID && cs.isDeleted == false).ToList();
            EmployeeVM employee = new EmployeeVM()
            {
                ServiceCenterList = services
            };
            return View(employee);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult createEmployee(EmployeeVM newEmp)
        {
            try
            {
                BeautyCenter center = getCenter();
                int centerID = center.ID;
                if (!ModelState.IsValid)
                {
                    return View(newEmp);
                }
                int empID = getEmployee(newEmp, centerID);

                foreach (var service in newEmp.ServiceCenterList)
                {
                    if (service.isSelected)
                    {
                        EmployeeDoService empDoService = new EmployeeDoService()
                        {
                            ServiceID = service.ServiceID,
                            EmployeeID = empID
                        };
                        context.EmployeeDoServices.Add(empDoService);
                        context.SaveChanges();
                    }
                }


                return RedirectToAction("getEmployess");
            }
            catch (Exception)
            {
                return View(newEmp);
            }


        }

        private int getEmployee(EmployeeVM newEmp, int centerID)
        {
            Employee emp = new Employee()
            {
                CenterID = centerID,
                Name = newEmp.Name
            };
            context.Employees.Add(emp);
            context.SaveChanges();
            return context.Employees.Max(e => e.ID);
        }

        public ActionResult DeleteEmployee(int id)
        {
            var emp = context.Employees.Where(e => e.ID == id).FirstOrDefault();
            emp.IsDeleted = true;
            context.Entry(emp).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
            var empServiceList = context.EmployeeDoServices.Where(e => e.EmployeeID == id).ToList();
            foreach (var item in empServiceList)
            {
                item.IsDeleted = true;
                context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
            return RedirectToAction("getEmployess");
        }

        public ActionResult EditEmployee(int id)
        {
            BeautyCenter center = getCenter();
            int centerID = center.ID;
            var services = context.CentersServices.Where(cs => cs.CenterID == centerID && cs.isDeleted == false).ToList();
            EmployeeVM empVM = new EmployeeVM();
            var emp = context.Employees.Where(e => e.ID == id).FirstOrDefault();
            empVM.Name = emp.Name;
            empVM.ID = emp.ID;
            empVM.ServiceCenterList = services;
            return View(empVM);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult EditEmployee(EmployeeVM newEmp)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(newEmp);
                }
                var emp = context.Employees.Where(e => e.ID == newEmp.ID).FirstOrDefault();
                emp.Name = newEmp.Name;
                context.Entry(emp).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

                var empServices = context.EmployeeDoServices.Where(c => c.EmployeeID == emp.ID).ToList();
                foreach (var empService in empServices)
                {
                    empService.IsDeleted = true;
                    context.Entry(empService).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                }

                foreach (var service in newEmp.ServiceCenterList)
                {
                    if (service.isSelected)
                    {
                        EmployeeDoService empDoService = new EmployeeDoService()
                        {
                            ServiceID = service.ServiceID,
                            EmployeeID = emp.ID
                        };
                        context.EmployeeDoServices.Add(empDoService);
                        context.SaveChanges();
                    }
                }

                return RedirectToAction("getEmployess");
            }
            catch (Exception)
            {
                return View(newEmp);
            }
        }
    }
}