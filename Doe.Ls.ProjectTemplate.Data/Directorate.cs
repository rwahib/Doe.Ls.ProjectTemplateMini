//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Doe.Ls.ProjectTemplate.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Directorate
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Directorate()
        {
            this.BusinessUnits = new HashSet<BusinessUnit>();
            this.FunctionalAreas = new HashSet<FunctionalArea>();
            this.Locations = new HashSet<Location>();
        }
    
        public int DirectorateId { get; set; }
        public string ExecutiveCod { get; set; }
        public string DirectorateName { get; set; }
        public string DirectorateDescription { get; set; }
        public string DirectorateOverview { get; set; }
        public string DirectorateCustomClass { get; set; }
        public int StatusId { get; set; }
        public int DirectorateOrder { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime LastModifiedDate { get; set; }
        public string LastModifiedBy { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BusinessUnit> BusinessUnits { get; set; }
        public virtual Executive Executive { get; set; }
        public virtual StatusValue StatusValue { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FunctionalArea> FunctionalAreas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Location> Locations { get; set; }
    }
}