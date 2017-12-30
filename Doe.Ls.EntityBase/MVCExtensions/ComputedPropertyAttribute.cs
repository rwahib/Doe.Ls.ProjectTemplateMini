using System;

namespace Doe.Ls.EntityBase.MVCExtensions
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ComputedPropertyAttribute : Attribute
    {
        public ComputedPropertyType ComputedPropertyType { get; set; }

        public object DefaultStatusValue { get; set; }

        public ComputedPropertyAttribute(ComputedPropertyType computedPropertyType, object defaultStatusValue = null)
        {
            ComputedPropertyType = computedPropertyType;
            DefaultStatusValue = defaultStatusValue;
        }
    }
}