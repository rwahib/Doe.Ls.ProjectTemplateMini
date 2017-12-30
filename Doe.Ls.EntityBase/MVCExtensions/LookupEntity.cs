using System;

namespace Doe.Ls.EntityBase.MVCExtensions {
    [AttributeUsage(AttributeTargets.Class)]
    public class LookupEntity : Attribute {
        public LookupEntityType LookupEntityType { get; set; }
    }
}