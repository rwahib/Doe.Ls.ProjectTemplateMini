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
    
    public partial class GeneralLog
    {
        public int LogId { get; set; }
        public string Action { get; set; }
        public string Context { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public string Username { get; set; }
        public int RoleId { get; set; }
        public string Note { get; set; }
    
        public virtual SysRole SysRole { get; set; }
    }
}