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
    
    public partial class PositionNote
    {
        public int PositionNoteId { get; set; }
        public int PositionId { get; set; }
        public string Notes { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime LastModifiedDate { get; set; }
    
        public virtual PositionInformation PositionInformation { get; set; }
    }
}
