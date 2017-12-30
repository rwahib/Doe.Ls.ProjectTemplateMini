using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doe.Ls.EntityBase.Helper
{
    public static class Utility
    {
        public static long GetRandId()
        {
            return DateTime.Now.Ticks;

        }
    }
}
