using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class BooleanExtension
    {
        public static bool IsTrue(this bool? value)
        {
            return value.HasValue && value.Value;
        }
    }
}
