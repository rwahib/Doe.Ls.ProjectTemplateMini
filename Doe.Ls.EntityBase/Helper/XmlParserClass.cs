using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Doe.Ls.EntityBase.Helper
{
    public class XmlParserClass
    {
        [XmlIgnore]
        public XmlParserClass Parent { get; set; }
        public string ClassName { get; set; }
        public List<XmlParserClass> XmlParserClassList { get; set; }
        public List<XmlParserItem> XmlParserItemList { get; set; }


        public override string ToString()
        {
            return
                $"{ClassName}-{(XmlParserClassList == null ? -1 : XmlParserClassList.Count)}-{(XmlParserItemList == null ? -1 : XmlParserItemList.Count)}";
        }
    }
}