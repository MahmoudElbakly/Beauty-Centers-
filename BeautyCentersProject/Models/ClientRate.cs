//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BeautyCentersProject.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ClientRate
    {
        public int ID { get; set; }
        public string ClientID { get; set; }
        public int CenterServiceID { get; set; }
        public int rate { get; set; }
        public string Comment { get; set; }
    
        public virtual CentersService CentersService { get; set; }
        public virtual Client Client { get; set; }
    }
}
