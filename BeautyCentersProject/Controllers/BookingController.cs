using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BeautyCentersProject.Models;
using BeautyCentersProject.ViewModel;

namespace BeautyCentersProject.Controllers
{
    public class BookingController : Controller
    {
        BeautyCenterDBEntities context=new BeautyCenterDBEntities();
        // GET: Booking
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BookingNow(int Centerid,string userName)
        {
                    ApplicationManager manager =
                        new ApplicationManager(new ApplicationStore(new ApplicationDbContext()));
                    ApplicationUser user = manager.FindByNameAsync(userName).Result;
                    List<EmployeeDoService> listofeEmployeeDoServices = new List<EmployeeDoService>();
                    var query = (from VAR in context.BeautyCenters where VAR.ID == Centerid select VAR)
                        .FirstOrDefault();
                    var employess = (from emp in context.Employees where emp.CenterID == Centerid select emp).ToList();
                    foreach (var VARIABLE in employess)
                    {
                        var employeedoservice =
                            (from empdoservice in context.EmployeeDoServices
                                where empdoservice.EmployeeID == VARIABLE.ID
                                select empdoservice).FirstOrDefault();
                        listofeEmployeeDoServices.Add(employeedoservice);

                    }

                    if (user != null)
                    {
                        BookingViewModel bookingViewModel = new BookingViewModel()
                        {
                            CentersServices = query.CentersServices.ToList(),
                            EmployeesDoService = listofeEmployeeDoServices,
                            Employees = employess,
                            CenterID = Centerid,
                            ClientPhone = user.PhoneNumber,
                            ClientID = user.Id,
                            CenterWorksHours =
                                (from c in context.CenterWorksHours where query.ID == c.CenterID select c).ToList()

                        };
                        return PartialView("BookingNow", bookingViewModel);
                    }
                

                return RedirectToAction("Login", "Account");
        }
        [Authorize]
        [HttpPost]
        public ActionResult ConfirmingBooking(BookingViewModel viewModel,List<int> Employee, List<int> ServicesID)
        {

            if (ServicesID.Count < 1)
            {
                RedirectToAction("BookingNow");
            }
            double TP=0;
            List<CentersService> selectedCentersServices=new List<CentersService>();
            var dateTime = viewModel.StartTime;
            //var date = dateTime;
            TimeSpan time= TimeSpan.Zero;
            foreach (var VARIABLE in ServicesID)
            {
                var centerservice = context.CentersServices.Where(s => s.ServiceID == VARIABLE).FirstOrDefault();
                selectedCentersServices.Add(centerservice);
                TP += centerservice.Price;
                time += centerservice.EstimatedTime;

            }
            DateTime TimeOfNow=DateTime.Now;
            
                Order order = new Order()
            {
                CenterID = viewModel.CenterID,
                ClientID = viewModel.ClientID,
                StartTime = viewModel.StartTime,
                EndTime = dateTime+time,
               // time = viewModel.Time.TimeOfDay,
                StatusID = 1,
                LastUpdate = TimeOfNow,
                TotalPrice = TP,
                ClientPhone = viewModel.ClientPhone
                
            };
            
            context.Orders.Add(order);
            context.SaveChanges();
            var lastaddorderID = context.Orders.Max(o => o.ID);
            for (int i = 0; i < ServicesID.Count; i++)
            {

                // insert 
                if (Employee[i] != 0) 
                {
                    OrderServiceEmployee orderServiceEmployee = new OrderServiceEmployee()
                    {
                        OrderID = lastaddorderID,
                        ServiceID = ServicesID[i],
                        EmployeeID = Employee[i],

                    };
                    context.OrderServiceEmployees.Add(orderServiceEmployee);
                }
                else
                {
                    OrderServiceEmployee orderServiceEmployee = new OrderServiceEmployee()
                    {
                        OrderID = lastaddorderID,
                        ServiceID = ServicesID[i],
                    };
                    context.OrderServiceEmployees.Add(orderServiceEmployee);
                }
              }
            context.SaveChanges();
            return RedirectToAction("GetOrders","Orders",new { userName = User.Identity.Name});
        }
    }
}