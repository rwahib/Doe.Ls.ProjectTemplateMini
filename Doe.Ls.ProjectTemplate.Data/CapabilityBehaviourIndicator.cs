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
    
    public partial class CapabilityBehaviourIndicator
    {
        public int CapabilityNameId { get; set; }
        public int CapabilityLevelId { get; set; }
        public string IndicatorContext { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime LastUpdated { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }
    
        public virtual CapabilityLevel CapabilityLevel { get; set; }
        public virtual CapabilityName CapabilityName { get; set; }
    }
}
