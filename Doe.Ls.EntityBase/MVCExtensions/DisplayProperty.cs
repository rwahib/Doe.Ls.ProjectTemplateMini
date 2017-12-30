using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doe.Ls.EntityBase.MVCExtensions {
    [AttributeUsage(AttributeTargets.Class)]
    public class DisplayPropertyAttribute :Attribute{
        public string PropertyName { get; set; }
        public bool Computed { get; set; }
        public string  Formula { get; set; } 
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class SecureAttribute : Attribute {
        public bool Secure { get; set; }
        public string Roles { get; set; }
    }
}
