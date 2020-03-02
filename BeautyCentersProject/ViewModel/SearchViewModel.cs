using BeautyCentersProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace BeautyCentersProject.ViewModel
{
    public class SearchViewModel
    {
        public List<City> BeautyCentersCities { get; set; } = new List<City>();
        public List<ServiceType> TypeOfService { get; set; } = new List<ServiceType>();
        public List<BeautyCenter> Centers { get; set; } = new List<BeautyCenter>();
    }
}