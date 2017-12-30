using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Doe.Ls.EntityBase.Helper
{
    public class XmlParser
    {
        public static XmlParser Parse(XDocument document)
        {
            var parser = new XmlParser();

            foreach (var el in document.Root.Elements())
            {
                if (el.Descendants().Any(de => de.Name.LocalName == "add"))
                {
                    var xmlParserClass = new XmlParserClass { ClassName = el.Name.LocalName };
                    if (parser.XmlParserClassList == null) parser.XmlParserClassList = new List<XmlParserClass>();
                    parser.XmlParserClassList.Add(xmlParserClass);
                    Parse(xmlParserClass, el);
                }
            }

            return parser;
        }

        public static void Parse(XmlParserClass @class, XElement element)
        {
            var propType = GetPropType(@class);
            if (element.Descendants().Any(de => de.Name.LocalName == "add"))
            {

                foreach (var el in element.Elements())
                {
                    if (el.Name.LocalName == "add")
                    {
                        if (@class.XmlParserItemList == null) @class.XmlParserItemList = new List<XmlParserItem>();
                        var item = new XmlParserItem { Parent = @class, PropertyType = propType };
                        if (el.Attributes("key").Any())
                        {
                            item.PropertyName = el.Attributes("key").First().Value;
                            if (propType == PropertyType.None) item.PropertyType = PropertyType.Key;
                        }
                        if (el.Attributes("name").Any())
                        {
                            item.PropertyName = el.Attributes("name").First().Value;
                            if (propType == PropertyType.None) item.PropertyType = PropertyType.Name;
                        }
                        @class.XmlParserItemList.Add(item);
                    }
                    else
                    {
                        if (el.Elements().Any())
                        {
                            var xmlParserClass = new XmlParserClass { ClassName = el.Name.LocalName, Parent = @class };
                            if (@class.XmlParserClassList == null)
                                @class.XmlParserClassList = new List<XmlParserClass>();
                            @class.XmlParserClassList.Add(xmlParserClass);
                            Parse(xmlParserClass, el);
                        }

                    }
                }


            }

        }

        private static PropertyType GetPropType(XmlParserClass @class)
        {
            if (@class.ClassName.ToLower().Contains("appsettings")) return PropertyType.AppSettings;
            if (@class.ClassName.ToLower().Contains("connectionstrings")) return PropertyType.ConnectionString;

            return PropertyType.None;

        }

        public List<XmlParserClass> XmlParserClassList { get; set; }


    }
}
