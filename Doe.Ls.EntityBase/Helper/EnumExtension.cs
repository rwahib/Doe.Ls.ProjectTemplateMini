using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace System
{
    public static class EnumExtension
    {
        public static T Parse<T>(string value)
        {
            return (T) Enum.Parse(typeof (T), value);
        }


        public static string[] GetNames<T>()
        {
            return Enum.GetNames(typeof (T));
        }


        public static int[] GetIntegerValues<T>() {
            var list = new List<int>();
            var result = Enum.GetValues(typeof(T));
            foreach (var val in result) {
                list.Add((int)val);
            }
            return list.ToArray();
        }
        /// <summary>
        /// Returns description attribute of 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            object[] attribs = field.GetCustomAttributes(typeof (DescriptionAttribute), true);
           
            if (attribs.Length > 0)
            {
                return ((DescriptionAttribute) attribs[0]).Description;
            }
            return value.ToString();
        }

        public static List<int> ToIntegerList<T>(this IEnumerable<T> list) 
        {
            return list.Select(item => (item as Enum).ToInteger()).ToList();
            
        }

        public static int ToInteger(this Enum value)
            {

            object val = Convert.ChangeType(value, value.GetTypeCode());
            return (int)val;

            }
        }
}