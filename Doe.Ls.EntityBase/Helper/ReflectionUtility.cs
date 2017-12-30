using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doe.Ls.EntityBase.Helper
{
    public static class ReflectionUtility
    {
        public static Attribute GetCustomAttribute<T, K>(string methodName) where T : class where K : Attribute
        {
            var property = typeof(T).GetMembers().FirstOrDefault(p => p.Name.Equals(methodName, StringComparison.OrdinalIgnoreCase));
            return property?.GetCustomAttributes(true).FirstOrDefault(at => (at is K)) as K;
        }
    }
}
