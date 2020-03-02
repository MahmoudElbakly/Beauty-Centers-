using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BeautyCentersProject.Models;
using BeautyCentersProject.ViewModel;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace BeautyCentersProject.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        BeautyCenterDBEntities context = new BeautyCenterDBEntities();

        // GET: Orders
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult GetOrders(string userName)
        {
            if (userName != null)
            {
                ApplicationManager manager =
                    new ApplicationManager(new ApplicationStore(new ApplicationDbContext()));
                ApplicationUser user = manager.FindByNameAsync(userName).Result;
                
                var CurrentUser = System.Web.HttpContext.Current.User.Identity.Name;
                if (CurrentUser != userName)
                {
                    return RedirectToAction("Login", "Account");
                }
        
                List<OrderServiceEmployee>listofOrderServiceEmployees=new List<OrderServiceEmployee>();
                List<Service> listofServices=new List<Service>();// list of services
                List<Offer> listofOffers=new List<Offer>();//list of offers
                orderViewModel orderViewModel = new orderViewModel();
                DateTime dateofnow=DateTime.Now;
                    // selcet orders
                    var orders = (from VAR in context.Orders
                        where VAR.ClientID == user.Id && VAR.StartTime >= dateofnow
                              select VAR).ToList();
                orderViewModel.Orders = orders; // fill list of orders
                foreach (var VARIABLE in orderViewModel.Orders)
                {   
                    // select orderService Employee
                    var orderServiceEmployee = (from serv in context.OrderServiceEmployees
                        where serv.OrderID == VARIABLE.ID
                        select serv).FirstOrDefault();
                    if (orderServiceEmployee != null)
                    {
                        listofOrderServiceEmployees.Add(orderServiceEmployee);
                        var service = (from serv in context.Services
                            where serv.ID == orderServiceEmployee.ServiceID
                            select serv).FirstOrDefault();
                        listofServices.Add(service);
                    }
                    var offer = (from row in context.Offers where row.OrderID == VARIABLE.ID && row.Order.StatusID!=3 select row).FirstOrDefault();
                    if (offer!=null)
                        {
                            listofOffers.Add(offer);
                        }

                }

                orderViewModel.ServiceEmployees = listofOrderServiceEmployees;
                orderViewModel.Services = listofServices;
                orderViewModel.Offers = listofOffers;
                return View(orderViewModel);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult CancelOrder(int orderID,string userName)
        {
           
            var order = (from VAR in context.Orders where VAR.ID == orderID select VAR).FirstOrDefault();
            var offers = (from of in context.Offers where order.ID == of.OrderID select of).ToList();
            List<Offer> listoOffersForThisOrder=new List<Offer>();
            listoOffersForThisOrder = offers;
            order.StatusID = 3;
            foreach (var offer in listoOffersForThisOrder)
            {
                var query = (from offer1 in context.Offers where offer1.ID == offer.ID select offer1).FirstOrDefault();
                context.Offers.Remove(query);
                context.SaveChanges();
            }
            context.SaveChanges();
           return RedirectToAction("GetOrders",new {userName=userName });
        }

        public ActionResult AcceptOffer(int offerID,int orderID,string userName)
        {
            var offer = (from of in context.Offers where of.ID == offerID select of).FirstOrDefault(); // choosen offer
            var order = (from or in context.Orders where or.ID == orderID select or).FirstOrDefault(); // choosen order
            var offers = (from VAR in context.Offers where VAR.OrderID == order.ID select VAR).ToList(); // list of offers for this order
            foreach (var var in offers)
            {
                context.Offers.Remove(var);
                context.SaveChanges();
            }
            order.StatusID = 2;
            order.StartTime = offer.StartTime;
            order.EndTime = offer.EndTime;
            context.SaveChanges();
            return RedirectToAction("GetOrders",new {userName=userName});
        }
    }
}