using BeautyCentersProject.Models;
using BeautyCentersProject.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautyCentersProject.ViewModel
{
    public class beautyCenterViewModel
    {
        public List<BeautyCenter> centerList { get; set; }
        public ApproviedClass selectApprovied { get; set; }
    }
}