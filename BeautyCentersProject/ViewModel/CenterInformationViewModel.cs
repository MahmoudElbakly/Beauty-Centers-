using BeautyCentersProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautyCentersProject.ViewModel
{
    public class CenterInformationViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string cityName { get; set; }
        public string Logo { get; set; }
        public int? Gender { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public int ClienId { get; set; }
        public  double? TotalRate { get; set; }
        public List<CentersService> Services { get; set; }
        public List<Employee> Employees { get; set; }
        public List<EmployeeDoService> EmployeeDoServices { get; set; }
        public List<CenterWorksHour> CenterWorksHours { get; set; }




    }
}