using System.Xml.Linq;

namespace System
{
    public static class XmlHelper
    {
        /// <summary>
        /// Will check teh null reference and will return empty
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string GetValue(this XElement element)
        {
            if (element == null) return string.Empty;

            return element.Value;
        }

    }
}
