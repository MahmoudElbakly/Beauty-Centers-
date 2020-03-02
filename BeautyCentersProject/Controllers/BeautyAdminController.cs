using BeautyCentersProject.Models;
using BeautyCentersProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BeautyCentersProject.Controllers
{
    public class BeautyAdminController : Controller
    {
        BeautyCenterDBEntities context = new BeautyCenterDBEntities();
        int centerId = 2;
        // GET: BeautyAdmin 
        [Authorize(Roles = "CenterAdmin")]
        public ActionResult Index()
        {
            var statusList = context.Status.ToList();
            return View(statusList);
        }
        [HttpPost]
        public ActionResult UpdateOrderStatus(string statusId, string ordId)
        {
            try
            {
                int orderID = int.Parse(ordId);
                int statusID = int.Parse(statusId);
                bool isUpdated = false;
                Order order = context.Orders.Where(o => o.ID == orderID).FirstOrDefault();
                if (order != null && statusID > 0)
                {
                    order.StatusID = statusID;
                    context.Entry(order).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    isUpdated = true;
                }
                return this.Json(new { updated = isUpdated }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return this.Json(new { updated = false }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult AddOfferForOrder(int ordId, DateTime start, DateTime end)
        {
            bool isAdded = false;
            try
            {
                if (ordId > 0)// && end > start)  // end must be lager than start
                {
                    Offer offer = new Offer { OrderID = ordId, StartTime = start, EndTime = end };
                    context.Entry(offer).State = System.Data.Entity.EntityState.Added;
                    context.Offers.Add(offer);
                    context.SaveChanges();
                    isAdded = true;
                }
                return this.Json(new { offerAdded = isAdded }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return this.Json(new { offerAdded = false, error = ex }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetOffersForOrder(int ordId)
        {
            var query = context.Offers
                .Where(of => of.OrderID == ordId)
                .Select(of => new {
                    offerId = of.ID,
                    ordId = of.OrderID,
                    start = of.StartTime,
                    end = of.EndTime
                }).Take(5).OrderByDescending(of=>of.offerId).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteOfferById(int offId)
        {
            var deleted = false;
            var offer = context.Offers.Where(of => of.ID == offId).FirstOrDefault();
            if(offer != null)
            {
                try
                {
                    context.Offers.Remove(offer);
                    context.SaveChanges();
                    deleted = true;
                }
                catch (Exception ex)
                {
                    return Json(new { deleted, error=ex.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { deleted }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetOrders(string userName)
        {
            var userId = context.AspNetUsers.Where(User => User.UserName == userName).Select(u => u.Id).FirstOrDefault();
            var centerId = context.CenterAdmins.Where(ad => ad.UserID == userId).Select(u => u.CenterID).FirstOrDefault();
          
            var query =
            (from ord in context.Orders
             where ord.CenterID == centerId
             select new {
                 id = ord.ID,
                 totalPrice = ord.TotalPrice, // orderTotalPrice(ord.ID),
                 status = context.Status.Where(s => s.ID == ord.StatusID).Select(s => s.Status1).FirstOrDefault(),
                 color = context.Status.Where(s => s.ID == ord.StatusID).Select(s => s.Color).FirstOrDefault(),
                 start = ord.StartTime,
                 end = ord.EndTime,
                 clientName = context.Clients.Where(cl => cl.UserID == ord.ClientID).Select(cl => cl.AspNetUser.Name)
             }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetOrderServies(int OID)
        {

            var query = (
                 from ordSer in context.OrderServiceEmployees
                 where ordSer.OrderID == OID && ordSer.Service.IsDeleted == false
                 select new {
                     srvId = ordSer.Service.ID,
                     name = ordSer.Service.Name,
                     price = context.CentersServices
                            .Where(s => s.ServiceID == ordSer.ServiceID)
                            .Select(s => s.Price).FirstOrDefault(),
                     empName = ordSer.Employee.Name,
                     empId = ordSer.EmployeeID
                 }).ToList();

            return this.Json(query, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSrvEmployee(int OID, int SrvID)
        {
            List<int> EmpsIds = new List<int>();
            var centerId = context.Orders.Where(ord => ord.ID == OID).Select(ord => ord.CenterID).FirstOrDefault();
            var centerEmpsIds = context.Employees.Where(emp => emp.CenterID == centerId).Select(emp => emp.ID).ToList();
            //var srvEmps = centerEmps.Where(e=>e.EmployeeDoServices)
            foreach (var empId in centerEmpsIds)
            {
                var srvEmpsId = (from EmpSrv in context.EmployeeDoServices
                                 where EmpSrv.EmployeeID == empId && EmpSrv.ServiceID == SrvID
                                 select EmpSrv.EmployeeID
                              ).FirstOrDefault();
                if (srvEmpsId != 0)
                {
                    EmpsIds.Add(srvEmpsId);
                }
            }
            List<EmployeeViewModel> EmpQuery = new List<EmployeeViewModel>();
            foreach (var empId in EmpsIds)
            {
                var emp = context.Employees.Where(e => e.ID == empId).Select(e => new EmployeeViewModel { ID = e.ID, Name = e.Name }).FirstOrDefault();
                EmpQuery.Add(emp);
            }
            return this.Json(EmpQuery, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCalenderByEmpID()
        {
            return PartialView("OrderCalender");
        }
        public ActionResult GetCalender()
        {
            return PartialView("OrderCalender2");
        }
        [HttpPost]
        public ActionResult GetOrdersInDay(string userName, DateTime start)
        {
            var startDate = start.Date;
            var endDate = startDate.AddDays(1);

            var userId = context.AspNetUsers.Where(User => User.UserName == userName).Select(u => u.Id).FirstOrDefault();
            var centerId = context.CenterAdmins.Where(ad => ad.UserID == userId).Select(u => u.CenterID).FirstOrDefault();

            var orders = (from ord in context.Orders
                         where ord.CenterID == centerId  && ord.StatusID == 2 && ord.StartTime >= startDate && ord.StartTime < endDate
                         select ord
                         ).ToList();

            var employees = context.Employees.Where(e => e.CenterID == centerId).Select(emp =>new { id = emp.ID, title = emp.Name }).ToList();
           
            List<OrderEmployee> ordersEmployee = new List<OrderEmployee>();
            foreach (var order in orders)
            {
                var eachOrderEmp = (from ose in context.OrderServiceEmployees
                                     where ose.OrderID == order.ID
                                     select ose).ToList();
                foreach (var emp in eachOrderEmp)
                {
                    ordersEmployee.Add(new OrderEmployee
                    {
                        title = order.ID,
                        start1 = order.StartTime,
                        end1 = order.EndTime,
                        resourceId = emp.EmployeeID,
                        client = order.Client.AspNetUser.Name
                    });
                }
            }

            return Json(new { ordersEmployee, employees }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateOrderServiceEmployee(List<OrderServiceEmployee> OrdSrvEmpList)
        {
            bool updated = false;
            string error = "";
            if (OrdSrvEmpList.Count > 0)
            {

                foreach (var OSE in OrdSrvEmpList)
                {
                    if (OSE.EmployeeID != null)
                    {

                        var orderServiceEmployee = context.OrderServiceEmployees
                            .Where(ose => ose.OrderID == OSE.OrderID && ose.ServiceID == OSE.ServiceID)
                            .FirstOrDefault();
                        if (orderServiceEmployee.EmployeeID == null)
                        {
                            orderServiceEmployee.EmployeeID = OSE.EmployeeID;
                        }
                    }
                }
                try
                {
                    context.SaveChanges();
                    updated = true;
                }
                catch (Exception ex)
                {
                    error = ex.ToString();
                } 
            }

            return Json(new { updated, error}, JsonRequestBehavior.AllowGet);
        }
        private string OrderTotalPrice(int oid)
        {
            var centerID = context.Orders.Where(o => o.ID == oid).Select(o => o.CenterID).FirstOrDefault();
            var PricesList = context.OrderServiceEmployees
                .Where(o => o.OrderID == oid)
                .Select(o => o.Service.CentersServices.Where(
                                        s => s.ServiceID == o.ServiceID && s.CenterID == centerID
                                        )).ToList();
            double totalPrice = 0;
            foreach (var centerServ in PricesList)
            {
                foreach (var pric in centerServ)
                {
                    totalPrice += pric.Price;
                }
            }
            return totalPrice.ToString();
        }

    }
}