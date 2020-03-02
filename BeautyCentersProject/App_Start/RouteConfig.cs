using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BeautyCentersProject
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
               name: "AcceptOffer",
               url: "Orders/AcceptOffer/{offerID}/{orderID}/{userName}",
               defaults: new { controller = "Orders", action = "AcceptOffer" }
           );
            routes.MapRoute(
                name: "CancelOrder",
                url: "Orders/CancelOrder/{orderID}/{userName}",
                defaults: new { controller = "Orders", action = "CancelOrder" }
            );
            routes.MapRoute(
                name: "GettingAcountOrders",
                url: "Orders/GetOrders/{userName}",
                defaults: new { controller = "Orders", action = "GetOrders" }
            );
            routes.MapRoute(
                name: "LogInBooking",
                url: "logBooking/{CenterID}",
                defaults: new { controller = "Account", action = "LogInForBooking" }
            );
            routes.MapRoute(
                name: "booking",
                url: "Booking/BookingNow/{Centerid}/{userName}",
                defaults: new { controller = "Booking", action = "BookingNow" }
            );

            routes.MapRoute(
                name: "settingcont",
                url: "Account/Setting/{Name}",
                defaults: new { controller = "Account", action = "Setting" }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
