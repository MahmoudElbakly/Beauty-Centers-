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
    using Shared;
    using System;
    using System.Collections.Generic;

    public partial class BeautyCenter
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BeautyCenter()
        {
            this.CenterAdmins = new HashSet<CenterAdmin>();
            this.CentersServices = new HashSet<CentersService>();
            this.CenterWorksHours = new HashSet<CenterWorksHour>();
            this.ClientCenterComments = new HashSet<ClientCenterComment>();
            this.Orders = new HashSet<Order>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public int CityID { get; set; }
        public string Logo { get; set; }
        public bool IsApproved { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<int> Gender { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public Nullable<double> TotalRate { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }

        public ApproviedClass Approvied { get; set; }

        public virtual City City { get; set; }
        public virtual ServiceType ServiceType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CenterAdmin> CenterAdmins { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CentersService> CentersServices { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CenterWorksHour> CenterWorksHours { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClientCenterComment> ClientCenterComments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
    }
}
