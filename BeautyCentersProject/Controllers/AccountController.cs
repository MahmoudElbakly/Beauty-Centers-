using BeautyCentersProject.Models;
using BeautyCentersProject.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BeautyCentersProject.Controllers
{
    public class AccountController : Controller
    {
        BeautyCenterDBEntities context = new BeautyCenterDBEntities();
        // GET: Account
        [Authorize(Roles = "Admin")]
        public ActionResult AdminRegisteration()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminRegisteration(AdminRegisterationViewModel userAdmin)
        {

            if (!ModelState.IsValid)
            {
                return View(userAdmin);
            }
            ApplicationUser admin = new ApplicationUser()
            {
                Name = userAdmin.Name,
                UserName = userAdmin.Username,
                Gender = userAdmin.Gender.ToString(),
                PasswordHash = userAdmin.Password,
                PhoneNumber = userAdmin.Phone,
                Email = userAdmin.Email
            };

            ApplicationDbContext DBContext = new ApplicationDbContext();
            ApplicationStore store = new ApplicationStore(DBContext);
            ApplicationManager manager = new ApplicationManager(store);
            IdentityResult Result = manager.CreateAsync(admin, userAdmin.Password).Result;
            var RoleResult = manager.AddToRoleAsync(admin.Id, "Admin").Result;

            if (Result.Succeeded)
            {
                IAuthenticationManager authentication = HttpContext.GetOwinContext().Authentication;
                SignInManager<ApplicationUser, string> signIn = new SignInManager<ApplicationUser, string>(manager, authentication); 
                return RedirectToAction("Index", "Home");
            }

            string error = "";
            foreach (var err in Result.Errors)
            {
                error += err + " ";
            }
            ModelState.AddModelError("", error);
            return View(userAdmin);
        }


        public ActionResult services()
        {
            var servicesDB = context.Services.Where(s => s.IsDeleted == false).ToList();
            //List<ServicesViewModel> serList = new List<ServicesViewModel>();
            //for (int i = 0; i < servicesDB.Count; i++)
            //{
            //    ServicesViewModel ser = new ServicesViewModel();
            //    ser.ID = servicesDB[i].ID;
            //    ser.Name = servicesDB[i].Name;
            //    serList.Add(ser);
            //}

            //servicsListViewModel services = new servicsListViewModel();
            //services.ServiceList = serList;
            servicsListViewModel services = new servicsListViewModel();
            services.services = servicesDB;

            return View(services);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult services(servicsListViewModel services)
        {
            var servicesDB = context.Services.Where(s => s.IsDeleted == false).ToList();
            services.services = servicesDB;
            return View(services);
        }

        [HttpPost]
        public ActionResult createTable(List<List<string>> arrayList)
        {
            Session.Clear();
            Session["Services"] = arrayList;
            return Json(new { success = true, data = "ok" });
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

        public ActionResult AdminCenterRegister()
        {
            AdminClientViewModel adminClientVM = new AdminClientViewModel();
            adminClientVM.Cities = context.Cities.ToList();
            adminClientVM.typeList = context.ServiceTypes.ToList();

            var servicesDB = context.Services.Where(s => s.IsDeleted == false).ToList();
            adminClientVM.services = servicesDB;

            return View(adminClientVM);
                
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AdminCenterRegister(AdminClientViewModel newCenter)
        {
            var servicesList = Session["Services"] as List<List<string>>;

            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "خطأ في ادخال البيانات من فضلك تأكد من البيانات جيدا");
                    newCenter.Cities = context.Cities.ToList();
                    newCenter.typeList = context.ServiceTypes.ToList();
                    var servicesDB = context.Services.Where(s => s.IsDeleted == false).ToList();

                    newCenter.services = servicesDB;
                    return View(newCenter);
                }
                // add logo into image folder.
                string ImageFileName;
                string fileName = Path.GetFileNameWithoutExtension(newCenter.file.FileName);
                string extension = Path.GetExtension(newCenter.file.FileName);
                fileName = fileName + DateTime.Now.Millisecond.ToString() + extension;
                ImageFileName = fileName;
                fileName = Path.Combine(Server.MapPath("~/Content/Images/"), fileName);
                newCenter.file.SaveAs(fileName);

                //add beauty center.
                addBeautyCenter(newCenter, ImageFileName);

                int CenterID = context.BeautyCenters.Max(c => c.ID);
                addServicesToCenter(servicesList, CenterID);
                // add admin user
                bool isSuccess = addAdminCenter(newCenter, CenterID);

                if (isSuccess)
                {

                    return RedirectToAction("Index", "CenterAdmin");
                }
                else
                {
                    newCenter.Cities = context.Cities.ToList();
                    newCenter.typeList = context.ServiceTypes.ToList();
                    var servicesDB = context.Services.Where(s => s.IsDeleted == false).ToList();

                    newCenter.services = servicesDB;
                    return View(newCenter);
                }

            }
            catch (Exception ex)
            {
                newCenter.Cities = context.Cities.ToList();
                newCenter.typeList = context.ServiceTypes.ToList();
                var servicesDB = context.Services.Where(s => s.IsDeleted == false).ToList();

                newCenter.services = servicesDB;
                return View(newCenter);
            }
        }

        private bool addServicesToCenter(List<List<string>> servicesList,int CenterID)
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

        private bool addAdminCenter(AdminClientViewModel newCenter, int centerID)
        {
            ApplicationUser admin = new ApplicationUser()
            {
                Name = newCenter.AdminName,
                UserName = newCenter.Username,
                PasswordHash = newCenter.Password,
                PhoneNumber = newCenter.Phone,
                Email = newCenter.Email
                
            };

            ApplicationDbContext DBContext = new ApplicationDbContext();
            ApplicationStore store = new ApplicationStore(DBContext);
            ApplicationManager manager = new ApplicationManager(store);
            IdentityResult Result = manager.CreateAsync(admin, newCenter.Password).Result;
            var RoleResult = manager.AddToRoleAsync(admin.Id, "CenterAdmin").Result;
            if (Result.Succeeded)
            {
                IAuthenticationManager authentication = HttpContext.GetOwinContext().Authentication;
                SignInManager<ApplicationUser, string> signIn = new SignInManager<ApplicationUser, string>(manager, authentication);
                signIn.SignIn(admin, false, false);
                // add admin info to center admin table
                CenterAdmin centerAdmin = new CenterAdmin()
                {
                    UserID = admin.Id,
                    NID = newCenter.NID,
                    CenterID = centerID,

                    NiDImage = newCenter.NID,
                };
                context.CenterAdmins.Add(centerAdmin);
                context.SaveChanges();
                return true;
            }
            else
            {
                string error = "";
                foreach (var err in Result.Errors)
                {
                    error += err + " ";
                }
                ModelState.AddModelError("", error);
                return false;
            }
        }

        private void addBeautyCenter(AdminClientViewModel newCenter,string ImageFileName)
        {
            BeautyCenter center = new BeautyCenter()
            {
                Name = newCenter.CenterName,
                CityID = newCenter.CityID,
                Logo = ImageFileName,
                Gender = newCenter.Gender,
                Description = newCenter.Description,
                Address = newCenter.Address,
                UpdatedDate = DateTime.Now
            };
            context.BeautyCenters.Add(center);
            context.SaveChanges();
        }

        public ActionResult ClientRegisteration()
        {
            ClientViewModel newClient = new ClientViewModel();
            newClient.Cities = context.Cities.ToList();
            return View(newClient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ClientRegisteration(ClientViewModel userClient)
        {

            if (!ModelState.IsValid)
            {
                userClient.Cities = context.Cities.ToList();
                return View(userClient);
            }
            ApplicationUser client = new ApplicationUser()
            {
                // remember to add gender 
                Name = userClient.Name,
                UserName = userClient.Username,
                Gender = userClient.Gender.ToString(),
                PasswordHash = userClient.Password,
                PhoneNumber = userClient.Phone,
                Email = userClient.Email
            };

            ApplicationDbContext DBContext = new ApplicationDbContext();
            ApplicationStore store = new ApplicationStore(DBContext);
            ApplicationManager manager = new ApplicationManager(store);
            IdentityResult Result = manager.CreateAsync(client, userClient.Password).Result;
            var RoleResult = manager.AddToRoleAsync(client.Id, "Client").Result;

            if (Result.Succeeded)
            {
                Client newClient = new Client() {
                    UserID = client.Id,
                    Address = userClient.Address,
                    CityID = userClient.CityID
                };
                context.Clients.Add(newClient);
                context.SaveChanges();
                IAuthenticationManager authentication = HttpContext.GetOwinContext().Authentication;
                SignInManager<ApplicationUser, string> signIn = new SignInManager<ApplicationUser, string>(manager, authentication);
                signIn.SignIn(client, false, false);
                return RedirectToAction("Index", "Home");
            }

            string error = "";
            foreach (var err in Result.Errors)
            {
                error += err + " ";
            }
            ModelState.AddModelError("", error);
            return View(userClient);
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel loginVM)
        {
            if (ModelState.IsValid == false)
            {
                return View(loginVM);
            }
            ApplicationManager manager = new ApplicationManager(new ApplicationStore(new ApplicationDbContext()));
            ApplicationUser user = manager.FindByNameAsync(loginVM.Username).Result;
            if (user == null)
            {
                ModelState.AddModelError("", "User name not valid");
            }
            var found = manager.CheckPasswordAsync(user, loginVM.Password).Result;
            if (found)
            {
                IAuthenticationManager authentication = HttpContext.GetOwinContext().Authentication;
                SignInManager<ApplicationUser, string> signManager = new SignInManager<ApplicationUser, string>(manager, authentication);
                signManager.SignIn(user, false, false);
                //var roleID = user.Roles.Select(se => se.RoleId).FirstOrDefault();
                //var isMaager = manager.IsInRole(user.Id, "Admin");

                var centerAdmin = context.CenterAdmins.Where(c => c.UserID == user.Id).FirstOrDefault();
                if (centerAdmin != null)
                {
                    Session["Center"] = null;
                    Session["Center"] = centerAdmin.CenterID;
                    return RedirectToAction("Manage", "CenterAdmin");
                }

                return RedirectToAction("Index", "Home");

            }

            ModelState.AddModelError("", "Password not valid");

            return View(loginVM);
        }
        [Authorize]
        public ActionResult Setting(string Name)
        {
            ClientViewModel userVM = new ClientViewModel();
            ApplicationManager manager = new ApplicationManager(new ApplicationStore(new ApplicationDbContext()));
            ApplicationUser user = manager.FindByNameAsync(Name).Result;
            userVM.ID = user.Id;
            userVM.Name = user.Name;
            userVM.Username = user.UserName;
            userVM.Phone = user.PhoneNumber;
            userVM.Email = user.Email;
            // we can update address later
            return View(userVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Setting(ClientViewModel userVM)
        {
            ApplicationManager manager = new ApplicationManager(new ApplicationStore(new ApplicationDbContext()));
            ApplicationUser user = manager.FindByIdAsync(userVM.ID).Result;
            user.Email = userVM.Email;
            user.PhoneNumber = userVM.Phone;
            var result = manager.UpdateAsync(user).Result;
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(userVM);
        }
        public ActionResult Logout()
        {
            IAuthenticationManager authentication = HttpContext.GetOwinContext().Authentication;

            authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogInForBooking(int CenterID)
        {
            LoginViewModel login = new LoginViewModel()
            {
                CenterID = CenterID
            };
            return PartialView("LogInForBooking", login);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogInForBooking(LoginViewModel loginVM)
        {
            if (ModelState.IsValid == false)
            {
                return View(loginVM);
            }
            ApplicationManager manager = new ApplicationManager(new ApplicationStore(new ApplicationDbContext()));
            ApplicationUser user = manager.FindByNameAsync(loginVM.Username).Result;
            if (user == null)
            {
                ModelState.AddModelError("", "User name not valid");
            }
            var found = manager.CheckPasswordAsync(user, loginVM.Password).Result;
            if (found)
            {
                IAuthenticationManager authentication = HttpContext.GetOwinContext().Authentication;
                SignInManager<ApplicationUser, string> signManager = new SignInManager<ApplicationUser, string>(manager, authentication);
                signManager.SignIn(user, false, false);

                return RedirectToAction("GetBeautyCenter", "BeautyCenter", loginVM.CenterID);
            }

            ModelState.AddModelError("", "Password not valid");

            return View(loginVM);
        }
        //[ChildActionOnly]
        public void TabClose()
        {
            IAuthenticationManager authentication = HttpContext.GetOwinContext().Authentication;

            authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }
    }
}