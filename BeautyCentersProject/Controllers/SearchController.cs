using BeautyCentersProject.Models;
using BeautyCentersProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Optimization;
using PagedList;

namespace BeautyCentersProject.Controllers
{
    public class SearchController :Controller
    {

        BeautyCenterDBEntities context = new BeautyCenterDBEntities();
        public ActionResult SearchForm(int? CID)
        {

            SearchViewModel GetCenters = new SearchViewModel();
            if (CID == null) { 
            GetCenters.BeautyCentersCities = context.Cities.ToList();
            GetCenters.TypeOfService = context.ServiceTypes.ToList();
            GetCenters.Centers = context.BeautyCenters.Where(c=>c.IsApproved == true && c.IsDeleted == false).ToList();
            ViewBag.CityName = new SelectList(GetCenters.BeautyCentersCities, "CID", "City1");
            ViewBag.CenterType = new SelectList(GetCenters.TypeOfService, "TypeID", "Type");
            ViewBag.Centers = new SelectList(GetCenters.Centers, "ID", "Name");
                SearchValuesViewModel SearchValues = new SearchValuesViewModel();
                int cityid = CID ?? default(int);
                SearchValues.CityID = cityid;
                SearchValues.TypeID = null;
                SearchValues.CenterID = null;
                Session["SearchValues"] = SearchValues;
            }
            if (CID != null)
            {
                GetCenters.BeautyCentersCities = context.Cities.ToList();
                GetCenters.TypeOfService = context.ServiceTypes.ToList();
                GetCenters.Centers = (from cit in context.BeautyCenters
                                     where cit.CityID == CID && cit.IsApproved == true && cit.IsDeleted == false
                                     select cit).ToList();
                ViewBag.CityName = new SelectList(GetCenters.BeautyCentersCities, "CID", "City1");
                ViewBag.CenterType = new SelectList(GetCenters.TypeOfService, "TypeID", "Type");
                ViewBag.Centers = new SelectList(GetCenters.Centers, "ID", "Name");
                SearchValuesViewModel SearchValues = new SearchValuesViewModel();
                int cityid = CID ?? default(int);
                SearchValues.CityID = cityid;
                SearchValues.TypeID = null;
                SearchValues.CenterID = null;
                Session["SearchValues"] = SearchValues;
            }


            return PartialView("_Searching" ,GetCenters);
        }
        [AcceptVerbs(HttpVerbs.Post|HttpVerbs.Get)]
        //[ValidateAntiForgeryToken]
        public ActionResult CenetrsDetails(int ? CID,int ?TypeID ,int ? ID,int? page)
        {
            
            page = page ?? 1;
            if (CID == 0)
            {
                CID = null;
            }

            if (TypeID == 0)
            {
                TypeID = null;
            }
            if (ID == 0)
            {
                ID = null;
            }
            

            var Centers = context.BeautyCenters.Where(c => c.IsApproved == true && c.IsDeleted == false).ToList();
            if (CID == null || TypeID == null || ID == null)
            {
                SearchValuesViewModel SearchValues = new SearchValuesViewModel();
                int cityid = CID ?? default(int);
                int typID = TypeID ?? default(int);
                int CentID = ID ?? default(int);
                SearchValues.CityID = cityid;
                SearchValues.TypeID = typID;
                SearchValues.CenterID = CentID;
                Session["SearchValues"] = SearchValues;
                Session["Centers"] = Centers;
            }
            if (CID != null || TypeID != null || ID != null)
            {
                SearchValuesViewModel SearchValues = new SearchValuesViewModel();
                int cityid = CID ?? default(int);
                int typID = TypeID ?? default(int);
                int CentID = ID ?? default(int);
                SearchValues.CityID = cityid;
                SearchValues.TypeID = typID;
                SearchValues.CenterID = CentID;
                Session["SearchValues"] = SearchValues;
                SearchValuesViewModel SearchVal = (SearchValuesViewModel)Session["SearchValues"];
                if (SearchVal.CityID != 0)
                {
                    Centers = Centers.Where(c => c.CityID == SearchVal.CityID).ToList();
                    Session["Centers"] = Centers;
                }

                if (SearchVal.TypeID != 0)
                {
                    Centers = Centers.Where(c => c.Gender == SearchVal.TypeID).ToList();
                    Session["Centers"] = Centers;
                }
                if (SearchVal.CenterID != 0)
                {
                    Centers = Centers.Where(c => c.ID == SearchVal.CenterID).ToList();
                    Session["Centers"] = Centers;
                }
            }

            List<BeautyCenter> FilteredCenters = new List<BeautyCenter>();
            List<BeautyCenter> OldCenters1 = (List<BeautyCenter>) Session["Centers"];
            if (Session["SRVIDS"] != null)
            {
                List<int> IDS = (List<int>)Session["SRVIDS"];
                if (IDS != null)
                {
                    foreach (var item in IDS)
                    {
                        var centerhaveservice = context.CentersServices.Where(s => s.ServiceID == item).Select(s => s.CenterID).ToList();
                        foreach (var Center in centerhaveservice)
                        {
                            var ctr = OldCenters1.Where(c => c.ID == Center).FirstOrDefault();
                            if (ctr != null) { FilteredCenters.Add(ctr); }


                        }
                    }
                }

            }
           
                if (FilteredCenters.Count != 0)
                {
                FilteredCenters= FilteredCenters.Distinct().ToList();


                }
                else
                {
                    FilteredCenters = OldCenters1.Distinct().ToList();
                }

            IPagedList<BeautyCenter> b = null;
            b = FilteredCenters.ToPagedList(page.Value, 3); 
            
            return View(b);
        }



        public ActionResult filterbyservice(int? CID, int? TypeID, int? ID, int? ServiceID, int? page)
        {
            
            page = page ?? 1;

            if (ServiceID == 0)
            {
                ServiceID = null;
            }
            if (CID == 0)
            {
                CID = null;
            }

            if (TypeID == 0)
            {
                TypeID = null;
            }
            if (ID == 0)
            {
                ID = null;
            }
            List<BeautyCenter> OldCenters1 = new List<BeautyCenter>();
            List<BeautyCenter> FilteredCenters = new List<BeautyCenter>();
            var Centers = context.BeautyCenters.Where(c => c.IsApproved == true && c.IsDeleted == false).ToList();

            if (CID == null || TypeID == null || ID == null)
            {
                
                OldCenters1 = Centers;
            }
            if (CID != null || TypeID != null || ID != null)
            {

                if (CID != null)
                {
                    OldCenters1  = OldCenters1.Where(c => c.CityID == CID).ToList();
                    

                }

                if (TypeID != null)
                {
                    OldCenters1 = OldCenters1.Where(c => c.Gender == TypeID).ToList();

                   

                }
                if (ID != null)
                {
                    OldCenters1 = OldCenters1.Where(c => c.ID == ID).ToList();
                  

                }
            }

            if (Session["SRVIDS"] == null)
            {

                if (ServiceID != null)
                {
                    List<int> ServiceIDS = new List<int>();
                    int SRVID = ServiceID ?? default(int);
                    ServiceIDS.Add(SRVID);
                    Session["SRVIDS"] = ServiceIDS;

                }
                List<int> IDS = (List<int>)Session["SRVIDS"];
                if (IDS != null)
                {
                    foreach (var item in IDS)
                    {
                        var centerhaveservice = context.CentersServices.Where(s => s.ServiceID == item).Select(s => s.CenterID).ToList();
                        foreach (var Center in centerhaveservice)
                        {
                            var ctr = OldCenters1.Where(c => c.ID == Center).FirstOrDefault();
                            if (ctr != null) { FilteredCenters.Add(ctr); }


                        }


                    }
                }

            }
            else
            {

                List<int> OlderIDs = (List<int>)Session["SRVIDS"];
                int SRVID = ServiceID ?? default(int);
                int rmvID = 0;

                foreach (var item in OlderIDs)
                {

                    if (item == SRVID)
                    {
                        rmvID = item;
                    }

                }
                if (rmvID != 0)
                {
                    OlderIDs.Remove(rmvID);
                    Session["SRVIDS"] = OlderIDs;
                }
                else
                {
                    if (SRVID != 0)
                    {
                        OlderIDs.Add(SRVID);
                        Session["SRVIDS"] = OlderIDs;
                    }
                }



                List<int> IDS = (List<int>)Session["SRVIDS"];
                if (IDS != null)
                {
                    foreach (var item in IDS)
                    {
                        var centerhaveservice = context.CentersServices.Where(s => s.ServiceID == item).Select(s => s.CenterID).ToList();
                        foreach (var Center in centerhaveservice)
                        {
                            var ctr = OldCenters1.Where(c => c.ID == Center).FirstOrDefault();
                            if (ctr != null) { FilteredCenters.Add(ctr); }


                        }


                    }
                }


                if (FilteredCenters.Count != 0)
                {
                    return PartialView("_Centers", FilteredCenters.Distinct().ToList());


                }
                else
                {
                    FilteredCenters = OldCenters1.Distinct().ToList();
                }


            }
            IPagedList<BeautyCenter> b = null;
            b = FilteredCenters.ToPagedList(page.Value, 3);

         

            return PartialView("_Centers", b);


        }
        public ActionResult Filteration()
        {

            List<BeautyCenter> centerstofilteration = new List<BeautyCenter>();
            centerstofilteration = Session["Centers"] as List<BeautyCenter>;
            List<int> centersID = new List<int>();
            foreach (var item in centerstofilteration)
            {
                var ID = item.ID;
                centersID.Add(ID);
            }
            List<Service> CenterServices = new List<Service>();
            foreach (var item in centersID)
            {
                var ServicesID = (from Csrv in context.CentersServices
                                  where Csrv.CenterID == item
                                  select Csrv.ServiceID).ToList();
                foreach (var srv in ServicesID)
                {
                    var service = (from SRV in context.Services
                                   where SRV.ID == srv
                                   select SRV).FirstOrDefault();
                    CenterServices.Add(service);
                }
            }


            var services = CenterServices.Distinct().ToList();
           

            return PartialView("_Filteration",services);
        }


        public JsonResult getcenters(int? CID)
        {

            if (CID != null)
            {
                var Center = (from cit in context.BeautyCenters
                               where cit.CityID == CID && cit.IsApproved == true && cit.IsDeleted == false
                              select new { cit.ID, cit.Name }).ToList();
                return this.Json(Center, JsonRequestBehavior.AllowGet);
            }


            var Centers = (from cit in context.BeautyCenters
                           where cit.IsApproved ==true && cit.IsDeleted ==false
                           select new { cit.ID, cit.Name }).ToList();

            return this.Json(Centers, JsonRequestBehavior.AllowGet);

        }

    }
}