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
    
    public partial class CapabilityLevel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CapabilityLevel()
        {
            this.CapabilityBehaviourIndicators = new HashSet<CapabilityBehaviourIndicator>();
            this.RoleCapabilities = new HashSet<RoleCapability>();
        }
    
        public int CapabilityLevelId { get; set; }
        public string LevelName { get; set; }
        public int DisplayOrder { get; set; }
        public int LevelOrder { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime LastUpdated { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CapabilityBehaviourIndicator> CapabilityBehaviourIndicators { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RoleCapability> RoleCapabilities { get; set; }
    }
}
