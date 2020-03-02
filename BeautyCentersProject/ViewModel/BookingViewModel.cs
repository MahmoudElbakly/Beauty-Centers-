using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BeautyCentersProject.Models;
using BeautyCentersProject.Shared;

namespace BeautyCentersProject.ViewModel
{
    public class BookingViewModel
    {
        [Required]
        public string ClientID { get; set; }
        public List<CentersService> CentersServices { get; set; }
        // [Required]
        public List<EmployeeDoService> EmployeesDoService  { get; set; }
        public List<CenterWorksHour> CenterWorksHours { get; set; }
        public List<Employee> Employees { get; set; }
        // public Days Days { get; set; }
         [Required]
        public int CenterID { get; set; }
       // [Required]
       // public DateTime Date { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public double TotalPrice { get; set; }
        public int StatusID  { get; set; }
        // public int ServiceID { get; set; }
        public int EmployeeID { get; set; }
        public string ClientPhone  { get; set; }






    }
}