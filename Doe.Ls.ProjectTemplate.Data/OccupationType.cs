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
    
    public partial class OccupationType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OccupationType()
        {
            this.PositionInformations = new HashSet<PositionInformation>();
        }
    
        public string OccupationTypeCode { get; set; }
        public string OccupationTypeName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PositionInformation> PositionInformations { get; set; }
    }
}
