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
    
    public partial class PositionFocusCriteria
    {
        public int PositionDescriptionId { get; set; }
        public int LookupId { get; set; }
        public string LookupCustomContent { get; set; }
        public string LastModifiedBy { get; set; }
        public System.DateTime LastModifiedDate { get; set; }
    
        public virtual LookupFocusGradeCriteria LookupFocusGradeCriteria { get; set; }
        public virtual PositionDescription PositionDescription { get; set; }
    }
}